using FluentValidation;

namespace FGC.Application.Deals.Commands.UpdateDeal
{
    
    public class UpdateDealCommandValidator : AbstractValidator<UpdateDealCommand>
    {
        public UpdateDealCommandValidator() 
        { 
            When(x => x.Discount != null, () =>
            {
                RuleFor(x => x.Discount.Value)
                    .GreaterThan(0)
                    .WithMessage("Discount must be greater than 0.")
                    .LessThanOrEqualTo(100)
                    .WithMessage("Discount must be less than or equal to 100.");
            });

            When(x => x.ExpirationDate != null, () =>
            {
                RuleFor(x => x.ExpirationDate.Value)
                    .NotEqual(default(DateTime))
                    .WithMessage("Expiration date must be a valid date.");
            });

            RuleFor(x => x.GameId)
                .NotNull().WithMessage("GameId cannot be null.")
                .Must(list => list!.Any()).WithMessage("At least one GameId is required.")
                .Must(list => list!.All(id => id.HasValue && id.Value > 0))
                .WithMessage("All GameIds must be valid integers.");


            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
        }
    }
}
