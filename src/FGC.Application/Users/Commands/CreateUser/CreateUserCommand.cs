using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Users;
using FGC.Domain.Users.Enums;
using FGC.Domain.Users.ValueObjects;
using MediatR;

namespace FGC.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {

            var user = new User
            {
                Email = new Email(request.Email),
                Name = request.Name,
                Password = new Password(request.Password, false),
                Username = request.Username,
                TypeUser = UserType.User
            };
             
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return (UserResponse)user;
        }
    }
}
