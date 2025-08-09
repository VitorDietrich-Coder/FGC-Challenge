using FGC.Application.Common;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using System.Text.Json.Serialization;

namespace FGC.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserResponse>
    {
        [JsonIgnore]
        public int Id { get; set; } 

        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserType? TypeUser { get; set; }
        public bool? Active { get; set; }
    }

    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Name = request.Name;
            entity.Username = request.Username;
            entity.Email = new Email(request.Email);
            entity.Password = new Password(request.Password);
            entity.TypeUser = (UserType)request.TypeUser;
            entity.Active = (bool)request.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return (UserResponse)entity;
        }
    }
}
