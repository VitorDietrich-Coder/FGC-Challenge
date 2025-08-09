using FluentValidation;
using System;

namespace FGC.Application.Users.Commands.UpdateUser.ReleaseUserGame
{
    public class ReleaseUserGameValidator : AbstractValidator<ReleaseUserGameCommand>
    {
        public ReleaseUserGameValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than zero.");

            RuleFor(x => x.GameId)
                .GreaterThan(0)
                .WithMessage("GameId must be greater than zero.");

            RuleFor(x => x.DateOfPurchase)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("DateOfPurchase cannot be in the future.");
        }
    }
}
