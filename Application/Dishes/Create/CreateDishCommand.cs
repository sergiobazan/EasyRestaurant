using Application.Abstractions.Messaging;

namespace Application.Dishes.Create;

public sealed record CreateDishCommand(CreateDishRequest Dish) : ICommand<Guid>;
