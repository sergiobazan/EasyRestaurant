using Infraestructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace Infraestructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
    };

    public ProcessOutboxMessagesJob(ApplicationDbContext context, IPublisher publisher, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Begin to process outbox message");

        List<OutboxMessage> messages = await GetOutboxMessages();

        if (messages.Count == 0)
        {
            _logger.LogInformation("Completed proccessing outbox messages - no messages to process");
            return;
        }

        foreach (OutboxMessage message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject(
                    message.Content, SerializerSettings)!;

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                _logger.LogError("Exception while processing outbox message {MessageId}", message.Id);

                message.Error = caughtException.Message;
            }

            message.ProcessOnUtc = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<OutboxMessage>> GetOutboxMessages()
    {
        return await _context
            .Set<OutboxMessage>()
            .Where(message => message.ProcessOnUtc == null)
            .OrderBy(message => message.OccurredOnUtc)
            .Take(29)
            .ToListAsync();
    }
}
