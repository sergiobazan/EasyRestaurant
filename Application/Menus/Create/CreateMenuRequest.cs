namespace Application.Menus.Create;

public sealed record CreateMenuRequest(
    string Name,
    DateTime Date);