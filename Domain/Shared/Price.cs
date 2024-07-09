using Domain.Abstractions;
using Domain.Dishes;

namespace Domain.Shared;

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

    public static Price operator +(Price first, Price second) => new(first.Amount + second.Amount);

    public static Price Zero() => new(0);

    public bool IsZero() => this == Zero();
}