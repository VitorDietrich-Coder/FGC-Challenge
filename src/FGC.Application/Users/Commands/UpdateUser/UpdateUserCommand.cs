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

            if (request.Name is not null)
            {
                entity.Name = request.Name;
            }

            if (request.Username is not null)
            {
                entity.Username = request.Username;
            }

            if (request.Email is not null)
            {
                entity.Email = new Email(request.Email);
            }

            if (request.Password is not null)
            {
                entity.Password = new Password(request.Password);
            }

            if (request.TypeUser is not null)
            {
                entity.TypeUser = (UserType)request.TypeUser;
            }

            if (request.Active is not null)
            {
                entity.Active = (bool)request.Active;
            }
      
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return (UserResponse)entity;
        }
    }
}
