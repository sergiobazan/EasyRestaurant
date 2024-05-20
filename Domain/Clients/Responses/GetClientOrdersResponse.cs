using Domain.Orders;

namespace Domain.Clients.Responses;

public sealed record GetClientOrdersResponse(
    Guid Id,
    string Name,
    List<ClientOrderResponse> Orders);

public sealed record ClientOrderResponse(
    Guid OrderId,
    Guid ClientId,
    DateTime Date,
    Status Status,
    List<ClientOrderDishes> Dishes);

public sealed record ClientOrderDishes(
    Guid DishId,
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    Domain.Dishes.Type Type,
    Domain.Dishes.Status Status);