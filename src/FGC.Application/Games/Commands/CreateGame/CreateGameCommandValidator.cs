using FGC.Application.Common;
using FluentValidation;
 

namespace FGC.Application.Games.Commands.CreateGame
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateGameCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Game name is required.")
                .Matches(@"^[A-Za-z0-9\s]+$")
                .WithMessage("Game name can only contain letters, numbers, and spaces.")
                .MaximumLength(100)
                .WithMessage("Game name must be at most 100 characters long.");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Genre is required.")
                .Matches(@"^[A-Za-z\s]+$")
                .WithMessage("Category can only contain letters and spaces.")
                .MaximumLength(100)
                .WithMessage("Category must be at most 100 characters long.");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required.")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.")
                .LessThanOrEqualTo(1000)
                .WithMessage("Price must be less than or equal to 1000.");

            RuleFor(x => x.SaleId)
                .GreaterThan(0)
                .When(x => x.SaleId.HasValue)
                .WithMessage("SaleId must be greater than 0 when provided.");
        }
    }
}