namespace Domain.Menus.Responses;

public sealed record MenuOrder(
    Guid MenuId,
    DateTime MenuDate,
    IEnumerable<OrderMenu> Orders);

public sealed record OrderMenu(
    Guid OrderId,
    DateTime OrderDate,
    Domain.Orders.Status OrderStatus,
    string OrderDescription,
    bool OrderPriority,
    Guid ClientId,
    string ClientName,
    IEnumerable<DishMenu> Dishes);

public sealed record DishMenu(
    Guid DishId,
    string DishName,
    string DishDescription,
    int DishQuantity,
    decimal DishPrice,
    Domain.Dishes.Status DishStatus);