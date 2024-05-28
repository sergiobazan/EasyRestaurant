using Domain.Abstractions;
using Domain.Dishes;
using Domain.Menus.DomainEvents;
using Domain.Orders;

namespace Domain.Menus;

public sealed class Menu : Entity
{
    private Menu()
    {
    }

    private Menu(Guid id, DateTime date) : base(id)
    {
        Date = date;
    }

    public DateTime Date { get; private set; }
    private readonly List<Dish> _dishes = new();
    public readonly List<Order> _orders = new();
    public IReadOnlyList<Dish> Dishes => _dishes.ToList();
    public IReadOnlyList<Order> Orders => _orders.ToList();

    public static Result<Menu> Create()
    {
        var menu = new Menu(Guid.NewGuid(), DateTime.UtcNow);

        menu.RaiseDomainEvent(new MenuCreatedDomainEvent(menu.Id));

        return menu;
    }

    public Result AddDishes(List<Dish> dishes)
    {
        _dishes.AddRange(dishes);

        return Result.Success();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }
}
