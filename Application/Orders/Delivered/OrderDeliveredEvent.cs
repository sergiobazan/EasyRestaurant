using MediatR;

namespace Application.Orders.Delivered;

public sealed record OrderDeliveredEvent(Guid OrderId) : INotification;