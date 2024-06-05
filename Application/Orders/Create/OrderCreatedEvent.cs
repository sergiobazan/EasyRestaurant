using MediatR;

namespace Application.Orders.Create;

public sealed record OrderCreatedEvent(Guid ClientId) : INotification;