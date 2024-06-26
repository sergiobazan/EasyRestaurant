﻿using Domain.Abstractions;
using Domain.Dishes.DomainEvents;
using Domain.Shared;

namespace Domain.Dishes;

public sealed class Dish : Entity
{
    private Dish() { }
    private Dish(
        Guid id,
        Guid menuId,
        Name name,
        Price price,
        Description description,
        Quantity quantity,
        Type dishType,
        Status status)
        : base(id)
    {
        MenuId = menuId;
        Name = name;
        Price = price;
        Description = description;
        Quantity = quantity;
        DishType = dishType;
        Status = status;
    }

    public Guid MenuId { get; private set; }
    public Name Name { get; private set; }
    public Price Price { get; private set; }
    public Description Description { get; private set; }
    public Quantity Quantity { get; private set; }
    public Type DishType { get; private set; }
    public Status Status { get; private set; }

    public static Result<Dish> Create(Guid menuId, Name name, Price price, Description description, Quantity quantity, Type dishType)
    {
        var dish = new Dish(Guid.NewGuid(), menuId, name, price, description, quantity, dishType, Status.Available);

        dish.RaiseDomainEvent(new DishCreatedDomainEvent(dish.Id));

        return dish;
    }

    public Result DecreaseQuantity()
    {
        var result = Quantity.Decrease();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Quantity = result.Value;

        return Result.Success();
    }

    public void IncreaseQuantity()
    {
        var result = Quantity.Increase();

        Quantity = result;

    }
}
