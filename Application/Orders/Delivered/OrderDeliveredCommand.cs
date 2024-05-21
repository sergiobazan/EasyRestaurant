using Application.Abstractions.Messaging;

namespace Application.Orders.Delivered;

public sealed record OrderDeliveredCommand(Guid OrderId) : ICommand;