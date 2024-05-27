using FluentValidation;

namespace Application.Dishes.Create;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(request => request.Dish.Name)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("Dish name can't be less than 4 characters");

        RuleFor(request => request.Dish.Description)
            .NotEmpty()
            .WithMessage("Dish description can't be empty");

        RuleFor(request => request.Dish.Price)
            .NotEmpty()
            .WithMessage("Dish price can't be empty");

        RuleFor(request => request.Dish.Quantity)
            .NotEmpty()
            .WithMessage("Dish quantity can't be empty");
    }
}
