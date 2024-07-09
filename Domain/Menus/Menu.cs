using Domain.Abstractions;
using Domain.Dishes;
using Domain.Menus.DomainEvents;
using Domain.Orders;
using Domain.Shared;

namespace Domain.Menus;

public sealed class Menu : Entity
{
    private Menu()
    {
    }
    private Menu(
        Guid id, 
        Name name,
        DateTime date) 
        : base(id)
    {
        Name = name;
        Date = date;
    }
    public Name Name { get; private set; }
    public DateTime Date { get; private set; }
    private readonly List<Dish> _dishes = new();
    public readonly List<Order> _orders = new();
    public IReadOnlyList<Dish> Dishes => _dishes.ToList();
    public IReadOnlyList<Order> Orders => _orders.ToList();

    public static Result<Menu> Create(Name name, DateTime date)
    {
        var menu = new Menu(Guid.NewGuid(), name, date);

        menu.RaiseDomainEvent(new MenuCreatedDomainEvent(menu.Id));

        return menu;
    }

    public Result AddDish(Dish dish)
    {
        _dishes.Add(dish);

        return Result.Success();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }
}
