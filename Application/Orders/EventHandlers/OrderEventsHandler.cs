using Application.Abstractions;
using Application.Orders.Cancel;
using Application.Orders.Change;
using Application.Orders.ChangePriority;
using Application.Orders.Create;
using Application.Orders.Delivered;
using MediatR;

namespace Application.Orders.EventHandlers;

public sealed class OrderEventsHandler : 
    INotificationHandler<OrderCreatedEvent>,
    INotificationHandler<OrderDeliveredEvent>,
    INotificationHandler<OrderPriorityChangedEvent>,
    INotificationHandler<OrderChangedEvent>,
    INotificationHandler<OrderCanceledEvent>
{
    private readonly ICacheService _cacheService;

    public OrderEventsHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        return HandleRemove(cancellationToken);
    }

    public Task Handle(OrderDeliveredEvent notification, CancellationToken cancellationToken)
    {
        return HandleRemove(cancellationToken);
    }

    public Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
    {
        return HandleRemove(cancellationToken);
    }

    public Task Handle(OrderPriorityChangedEvent notification, CancellationToken cancellationToken)
    {
        return HandleRemove(cancellationToken);
    }

    public Task Handle(OrderChangedEvent notification, CancellationToken cancellationToken)
    {
        return HandleRemove(cancellationToken);
    }

    public async Task HandleRemove(CancellationToken cancellationToken = default)
    {
        await _cacheService.RemoveByPartialKey("order", cancellationToken);
    }
}
