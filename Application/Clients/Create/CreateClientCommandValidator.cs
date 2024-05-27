using FluentValidation;

namespace Application.Clients.Create;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(request => request.Client.Phone)
            .NotEmpty()
            .Length(9)
            .WithMessage("Phone number should have 9 characters");

        RuleFor(request => request.Client.Gender)
            .NotEmpty()
            .Length(1)
            .WithMessage("Gender should be one character long Male (M) or Female (F)");

        RuleFor(request => request.Client.Name)
            .NotEmpty();
    }
}
