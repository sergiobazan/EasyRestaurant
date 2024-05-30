using Domain.Abstractions;
using Infraestructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Infraestructure.Interceptors;

public class OutboxInterceptor : SaveChangesInterceptor
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is not null)
        {
            SaveOutboxMessages(context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public static void SaveOutboxMessages(DbContext context)
    {

        var outboxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e =>
            {
                var domainEvents = e.DomainEvents;

                e.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings),
                OccurredOnUtc = DateTime.UtcNow
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);

    }
}
