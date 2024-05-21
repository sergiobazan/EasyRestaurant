using Domain.Abstractions;

namespace Domain.Orders;

public static class OrderErrors
{
    public static Error OrderNotFound(Guid id) => new(
        "Orders.OrderNotFound", $"Order with Id = {id} was not found.");

    public static Error OrderCanNotBeDelivered => new(
       "Orders.OrderCanNotBeDelivered", "The order can not be delivered");

    public static Error OrderCanNotBeCanceled => new(
      "Orders.OrderCanNotBeCanceled", "The order can not be canceled");
}
