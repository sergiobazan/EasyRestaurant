using Application.Abstractions.Messaging;


namespace Application.Orders.Create;

public sealed record CreateOrderCommand(Guid ClientId, List<Guid> DishIds) : ICommand<Guid>;