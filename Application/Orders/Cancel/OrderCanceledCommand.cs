using Application.Abstractions.Messaging;

namespace Application.Orders.Cancel;

public sealed record OrderCanceledCommand(Guid OrderId) : ICommand;

