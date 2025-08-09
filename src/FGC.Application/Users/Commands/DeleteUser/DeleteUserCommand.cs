using FGC.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGC.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler
        : IRequest<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            _context.Users.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
