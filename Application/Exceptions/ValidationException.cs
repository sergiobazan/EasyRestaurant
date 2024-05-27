namespace Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<ValidationError> error)
        : base("Validation failed")
    {
        Errors = error;
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }
}

public record ValidationError(string PropertyName, string ErrorMessage);