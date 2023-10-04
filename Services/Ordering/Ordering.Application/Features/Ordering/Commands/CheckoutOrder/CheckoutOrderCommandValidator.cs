using FluentValidation;

namespace Ordering.Application.Features.Ordering.Commands.CheckoutOrder;
public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(checkoutOrderCommand => checkoutOrderCommand.UserName)
            .NotEmpty().WithMessage("{UserName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must be less than 50 characters");

        RuleFor(checkoutOrderCommand => checkoutOrderCommand.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");
    }
}
