using MediatR;

namespace Application.Orders.ChangePriority;

public sealed record OrderPriorityChangedEvent(Guid OrderId) : INotification;