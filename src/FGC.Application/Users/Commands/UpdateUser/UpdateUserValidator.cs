using FluentValidation;

namespace FGC.Application.Users.Commands.UpdateUser
{
    
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("Name must be less than or equal to 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .MaximumLength(100)
                .WithMessage("Email must be less than or equal to 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100)
                .WithMessage("Password must be less than or equal to 100 characters.")
                .Matches(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least one letter, one number, and one special character.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.TypeUser)
                .IsInEnum()
                .WithMessage("Type must be a valid enum value.")
                .When(x => x.TypeUser.HasValue);
            
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today)
                .WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.Today.AddYears(-120))
                .WithMessage("Date of birth must be within the last 120 years.");

            RuleFor(x => x.Active)
                .NotNull()
                .WithMessage("Active status is required.")
                .When(x => x.Active.HasValue);

            RuleFor(x => x)
                .Must(x =>
                    !string.IsNullOrWhiteSpace(x.Name) ||
                    !string.IsNullOrWhiteSpace(x.Email) ||
                    !string.IsNullOrWhiteSpace(x.Password) ||
                    x.TypeUser.HasValue ||
                    x.Active.HasValue
                )
                .WithMessage("At least one field must be provided.");
        }
    }
}
