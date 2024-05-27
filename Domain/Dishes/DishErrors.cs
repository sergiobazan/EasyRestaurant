using Domain.Abstractions;

namespace Domain.Dishes;

public static class DishErrors
{
    public static Error NegativePrice => new(
        "Price.NegativePrice", "Price can not be negative");

    public static Error NegativeQuantity => new(
        "Quantity.NegativeQuantity", "Quantity can not be negative");

    public static Error DishNotFound(Guid id) => new(
        "Dish.DishNotFound", $"Dish with Id = {id} was not found.");

    public static Error DishOutOfStock => new(
        "Dish.DishOutOfStock", "Dish out of stock.");

    public static Error DishesNotFound => new(
        "Dish.DishesNotFound", "Dishes with given Ids were not found");
}
