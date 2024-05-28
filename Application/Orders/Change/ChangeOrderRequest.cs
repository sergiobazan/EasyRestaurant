namespace Application.Orders.Change;

public sealed record ChangeOrderRequest(
    Guid OrderId,
    Guid DishIdOld,
    Guid DishIdNew);
