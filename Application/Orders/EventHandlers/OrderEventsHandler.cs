using Application.Abstractions;
using Application.Orders.Create;
using MediatR;

namespace Application.Orders.EventHandlers;

public sealed class OrderEventsHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly ICacheService _cacheService;

    public OrderEventsHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync($"client-{notification.ClientId}-orders", cancellationToken);
    }
}
