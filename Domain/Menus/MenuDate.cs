using Domain.Abstractions;

namespace Domain.Menus;

public sealed record MenuDate
{
    public DateTime Value { get; init; }
    private MenuDate(DateTime value) => Value = value;
    public static Result<MenuDate> Create(DateTime value)
    {
        return new MenuDate(value);
    }
}