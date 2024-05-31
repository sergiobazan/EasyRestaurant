using Domain.Abstractions;

namespace Domain.Menus;

public static class MenuErrors
{
    public static Error MenuNotFound(Guid id) => new(
        "Menu.MenuNotFound", $"Menu with Id = {id} was not found");

    public static Error MenuDateCantBeEarlyThanToday => new(
        "Menu.MenuDateCantBeEarlyThanToday", "Menu date must not be earlier than today");
}
