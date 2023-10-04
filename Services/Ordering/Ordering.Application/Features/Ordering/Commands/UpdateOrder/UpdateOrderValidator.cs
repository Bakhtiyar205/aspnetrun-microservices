using FluentValidation;

namespace Ordering.Application.Features.Ordering.Commands.UpdateOrder;
public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(updateOrderCommand => updateOrderCommand.UserName)
            .NotEmpty().WithMessage("{UserName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must be less than 50 characters");

        RuleFor(updateOrderCommand => updateOrderCommand.EmailAddress)
          .NotEmpty().WithMessage("{EmailAddress} is required.");

        RuleFor(updateOrderCommand => updateOrderCommand.TotalPrice)
          .NotEmpty().WithMessage("{TotalPrice} is required.")
          .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");

    }
}
