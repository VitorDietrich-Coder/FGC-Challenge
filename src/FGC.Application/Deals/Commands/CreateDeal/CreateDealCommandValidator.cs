using FluentValidation;

namespace FGC.Application.Deals.Commands.CreateDeal
{
    
    public class CreatedealCommandValidator : AbstractValidator<CreateDealCommand>
    {
        public CreatedealCommandValidator()
        {
            RuleFor(x => x.Discount)
                .NotEmpty().WithMessage("Discount is required.")
                .GreaterThan(0).WithMessage("Discount must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Discount must be less than or equal to 100.");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty().WithMessage("Expiration date is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.");

            RuleFor(x => x.GameId)
                .Cascade(CascadeMode.Stop)
                .Must(list => list == null || list.All(id => id.HasValue && id.Value > 0)).WithMessage("All GameIds must be non-null.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
        }
    }
}
