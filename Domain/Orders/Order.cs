using Domain.Abstractions;
using Domain.Dishes;
using Domain.Orders.DomainEvents;
using Domain.Shared;

namespace Domain.Orders;

public sealed class Order : Entity
{
    private Order() { }
    private Order(
        Guid id,
        Guid menuId,
        Guid clientId,
        List<Dish> dishes,
        DateTime date,
        Status status,
        Description? description,
        Price totalPrice)
        : base(id)
    {
        MenuId = menuId;
        ClientId = clientId;
        _dishes = dishes;
        Date = date;
        Status = status;
        Description = description;
        TotalPrice = totalPrice;
    }
    public Guid MenuId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime Date { get; private set; }
    public Status Status { get; private set; }
    public Description? Description { get; private set; }
    public bool IsPriority { get; private set; } = false;
    public Price TotalPrice { get; private set; }

    private readonly List<Dish> _dishes = new();
    public List<Dish> Dishes => _dishes.ToList();

    public static Result<Order> Create(
        Guid clientId, 
        Guid menuId, 
        Description? description,
        List<Dish> dishes,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(dishes);

        var order = new Order(
            Guid.NewGuid(), 
            menuId, 
            clientId, 
            dishes,
            DateTime.UtcNow, 
            Status.Order, 
            description, 
            pricingDetails.TotalPrice);

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddDishes(List<Dish> dishes)
    {
        _dishes.AddRange(dishes);
    }

    public Result OrderServed()
    {
        if (Status != Status.Cancel && Status != Status.Delivered)
        {
            Status = Status.Delivered;

            RaiseDomainEvent(new OrderServedDomainEvent(Id));
            
            return Result.Success();
        }

        return Result.Failure(OrderErrors.OrderCanNotBeDelivered);
    }

    public Result OrderCanceled()
    {
        if (Status != Status.Cancel && Status != Status.Delivered)
        {
            Status = Status.Cancel;

            foreach (var dish in Dishes)
            {
                dish.IncreaseQuantity();
            }

            RaiseDomainEvent(new OrderCanceledDomainEvent(Id));

            return Result.Success();
        }

        return Result.Failure(OrderErrors.OrderCanNotBeDelivered);
    }

    public Result UpdateDish(Dish oldDish, Dish newDish)
    {
        if (!_dishes.Remove(oldDish))
        {
            return Result.Failure(DishErrors.DishNotFound(oldDish.Id));
        }

        if (_dishes.Contains(newDish))
        {
            return Result.Failure(DishErrors.DishAlreadyInOrder);
        }

        _dishes.Add(newDish);

        return Result.Success();
    }

    public void ChangePriority()
    {
        IsPriority = true;
    }
}
