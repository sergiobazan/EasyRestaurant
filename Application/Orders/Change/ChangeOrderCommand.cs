using Application.Abstractions.Messaging;

namespace Application.Orders.Change;

public sealed record ChangeOrderCommand(ChangeOrderRequest ChangeOrderRequest) : ICommand<Guid>;
