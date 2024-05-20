using Application.Abstractions.Messaging;


namespace Application.Orders.Create;

public sealed record CreateOrderCommand(Guid ClientId, string? Description, List<Guid> DishIds) : ICommand<Guid>;