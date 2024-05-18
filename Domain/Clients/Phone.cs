using Domain.Abstractions;
using Domain.Expretions;

namespace Domain.Clients;

public sealed record Phone
{
    private Phone(string prefix, string value)
    {
        Prefix = prefix;
        Value = value;
    }

    public string Prefix { get; init; }
    public string Value { get; init; }

    public static Result<Phone> Create(string prefix, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(prefix, nameof(prefix));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        if (!CustomExpretions.IsValidPrefix(prefix))
        {
            return Result.Failure<Phone>(ClientErrors.InvalidPrefix);
        }

        if (!CustomExpretions.IsValidPhoneNumber(value))
        {
            return Result.Failure<Phone>(ClientErrors.InvalidPhoneNumber);
        }

        return new Phone(prefix, value);
    }
}