using Application.Abstractions.Messaging;

namespace Application.Menus.Create;

public sealed record CreateMenuCommand(CreateMenuRequest Menu) : ICommand<Guid>;
