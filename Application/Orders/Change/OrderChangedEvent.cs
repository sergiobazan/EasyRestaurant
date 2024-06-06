using MediatR;

namespace Application.Orders.Change;

public sealed record OrderChangedEvent(Guid OrderId) : INotification;

