using Application.Abstractions.Messaging;

namespace Application.Orders.ChangePriority;

public sealed record ChangePriorityCommand(Guid OrderId) : ICommand;
