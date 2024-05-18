using Domain.Abstractions;

namespace Domain.Dishes;

public sealed record Price
{
    public decimal Amount { get; init; }
    private Price(decimal amount)
    {
        Amount = amount;
    }

    public static Result<Price> Create(decimal amount)
    {
        if (amount < 0) return Result.Failure<Price>(DishErrors.NegativePrice);
        
        return new Price(amount);
    }
}