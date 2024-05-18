using Domain.Abstractions;
using Domain.Dishes;

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
    public List<Dish> Dishes => _dishes.ToList();

    public static Result<Menu> Create()
    {
        var menu = new Menu(Guid.NewGuid(), DateTime.Now);

        return menu;
    }

    public Result AddDishes(List<Dish> dishes)
    {
        _dishes.AddRange(dishes);

        return Result.Success();
    }
}
