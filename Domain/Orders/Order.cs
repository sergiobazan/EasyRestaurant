﻿using Domain.Abstractions;
using Domain.Dishes;
using Domain.Shared;

namespace Domain.Orders;

public sealed class Order : Entity
{
    private Order() { }
    private Order(
        Guid id,
        Guid clientId,
        DateTime date,
        Status status,
        Description? description)
        : base(id)
    {
        ClientId = clientId;
        Date = date;
        Status = status;
        Description = description;
    }

    public Guid ClientId { get; private set; }
    public DateTime Date { get; private set; }
    public Status Status { get; private set; }
    public Description? Description { get; private set; }
    private readonly List<Dish> _dishes = new();
    public List<Dish> Dishes => _dishes.ToList();

    public static Result<Order> Create(Guid clientId, Description? description)
    {
        var order = new Order(Guid.NewGuid(), clientId, DateTime.UtcNow, Status.Order, description);

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddDishes(List<Dish> dishes)
    {
        _dishes.AddRange(dishes);
    }
}
