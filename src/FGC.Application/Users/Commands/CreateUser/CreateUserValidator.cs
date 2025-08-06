using FGC.Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGC.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator
        : AbstractValidator<CreateUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserCommandValidator(
            IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Email)
                .NotEmpty();

            RuleFor(v => v.Username)
                .NotEmpty();

            RuleFor(v => v.Password)
                .NotEmpty();
        }

        public async Task BeUniqueTitle(string title,
            CancellationToken cancellationToken)
        {
         
        }
    }
}
