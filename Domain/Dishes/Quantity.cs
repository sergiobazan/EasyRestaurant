using Domain.Abstractions;

namespace Domain.Dishes;

public sealed record Quantity
{
    public int Value { get; init; }
    private Quantity(int value)
    {
        Value = value;
    }

    public static Result<Quantity> Create(int value)
    {
        if (value < 0) return Result.Failure<Quantity>(DishErrors.NegativeQuantity);

        return new Quantity(value);
    }
}