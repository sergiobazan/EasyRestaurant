namespace Application.Dishes.Create;

public sealed record CreateDishRequest(
    Guid MenuId,
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    Domain.Dishes.Type Type);