using MediatR;

namespace Application.Orders.Cancel;

public sealed record OrderCanceledEvent(Guid OrderId) : INotification;