﻿using FluentValidation;

namespace FGC.Application.Auth.Commands
{
    
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .WithMessage("Email is required.")
              .EmailAddress()
              .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");           
        }
    }
}
