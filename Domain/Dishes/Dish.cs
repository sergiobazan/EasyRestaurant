using Domain.Abstractions;
using Domain.Shared;

namespace Domain.Dishes;

public sealed class Dish : Entity
{
    private Dish() { }
    private Dish(
        Guid id,
        Name name,
        Price price,
        Description description,
        Quantity quantity,
        Type dishType,
        Status status)
        : base(id)
    {
        Name = name;
        Price = price;
        Description = description;
        Quantity = quantity;
        DishType = dishType;
        Status = status;
    }

    public Name Name { get; private set; }
    public Price Price { get; private set; }
    public Description Description { get; private set; }
    public Quantity Quantity { get; private set; }
    public Type DishType { get; private set; }
    public Status Status { get; private set; }

    public static Result<Dish> Create(Name name, Price price, Description description, Quantity quantity, Type dishType)
    {
        var dish = new Dish(Guid.NewGuid(), name, price, description, quantity, dishType, Status.Available);

        dish.RaiseDomainEvent(new DishCreatedDomainEvent(dish.Id));

        return dish;
    }
}
