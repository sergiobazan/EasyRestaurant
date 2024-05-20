namespace Application.Dishes.Create;

public sealed record CreateDishRequest(
    string Name,
    decimal Price,
    string Description,
    int Quantity,
    Domain.Dishes.Type Type);