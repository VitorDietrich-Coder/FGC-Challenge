using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Users;
using FGC.Domain.Users.Enums;
using FGC.Domain.Users.ValueObjects;
using MediatR;

namespace FGC.Application.Users.Commands.CreateAdmin
{
    public class CreateAdminCommand : IRequest<UserResponse>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType TypeUser { get; set; }
        public bool Active { get; set; }
    }

    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, UserResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateAdminCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> Handle(CreateAdminCommand request,
            CancellationToken cancellationToken)
        {
             var user = new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = new Email(request.Email),
                Password = new Password(request.Password),
                TypeUser = UserType.Admin
            };
             
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return (UserResponse)user;
        }
    }
}
