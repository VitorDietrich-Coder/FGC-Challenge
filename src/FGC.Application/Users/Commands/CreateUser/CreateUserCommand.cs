using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;

namespace FGC.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
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
            var userEntity = _context.Users.Where(x => x.Name == request.Name || x.Username == request.Username || x.Email.Address == request.Email).FirstOrDefault();

            if (userEntity != null)
            {
                throw new InvalidOperationException($"The user {request.Name} already exists, Verify Email, Name or Username");
            }

            var user = new User
            {
                Email = new Email(request.Email),
                Name = request.Name,
                Password = new Password(request.Password),
                Username = request.Username,
                TypeUser = UserType.User,
                Active = false,
            };
             
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return (UserResponse)user;
        }
    }
}
