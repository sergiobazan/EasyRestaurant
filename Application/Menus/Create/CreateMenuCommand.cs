using Application.Abstractions.Messaging;

namespace Application.Menus.Create;

public sealed record CreateMenuCommand(List<Guid> DishIds) : ICommand<Guid>;