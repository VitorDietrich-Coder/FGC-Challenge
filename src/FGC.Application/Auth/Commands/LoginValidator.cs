using FluentValidation;

namespace FGC.Application.Auth.Commands
{
    
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
              .NotEmpty()
              .WithMessage("Username is required.")
              .EmailAddress()
              .WithMessage("Username must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");           
        }
    }
}
