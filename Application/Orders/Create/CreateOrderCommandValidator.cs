using FluentValidation;

namespace Application.Orders.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(order => order.ClientId)
            .NotEmpty()
            .WithMessage("Client Id can't be empty");

        RuleFor(order => order.DishIds)
           .NotEmpty()
           .WithMessage("Client Id can't be empty");
    }
}
