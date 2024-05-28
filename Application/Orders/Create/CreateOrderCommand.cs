using Application.Abstractions.Messaging;


namespace Application.Orders.Create;

public sealed record CreateOrderCommand(Guid ClientId, Guid MenuId, string? Description, List<Guid> DishIds) : ICommand<Guid>;