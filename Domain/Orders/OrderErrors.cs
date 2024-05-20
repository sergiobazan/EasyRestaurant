using Domain.Abstractions;

namespace Domain.Orders;

public static class OrderErrors
{
    public static Error CanNotAddDishes => new(
        "Orders.CanNotAddDishes", "Can't add dishes");
}
