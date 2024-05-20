using Domain.Abstractions;

namespace Domain.Dishes;

public static class DishErrors
{
    public static Error NegativePrice => new(
        "Price.NegativePrice", "Price can not be negative");

    public static Error NegativeQuantity => new(
        "Price.NegativeQuantity", "Quantity can not be negative");

    public static Error DishNotFound(Guid id) => new(
        "Price.DishNotFound", $"Dish with Id = {id} was not found.");

    public static Error DishOutOfStock => new(
        "Price.DishOutOfStock", "Dish out of stock.");
}
