using Domain.Abstractions;

namespace Domain.Clients;

public sealed record Gender
{
    private static readonly List<string> _genders = ["M", "F"];
    public string Value { get; init; }

    private Gender(string value) => Value = value;

    public static Result<Gender> Create(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        if (!_genders.Contains(value))
        {
            return Result.Failure<Gender>(ClientErrors.OnlyTwoGenders);
        }

        return new Gender(value);
    }
}