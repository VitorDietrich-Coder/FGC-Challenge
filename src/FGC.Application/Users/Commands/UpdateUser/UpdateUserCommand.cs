using FGC.Application.Common;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Users.Enums;
using FGC.Domain.Users.ValueObjects;
using System.Text.Json.Serialization;

namespace FGC.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; } 

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserType? TypeUser { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool? Active { get; set; }
    }

    public class UpdateUserCommandHandler
        : AsyncRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FindAsync(new object[] { request.Name }, cancellationToken);

            Guard.Against.NotFound(request.Name, entity);

            entity.Name = request.Name;
            entity.Email = new Email(request.Email);
            entity.Password = new Password(request.Password, false);
            entity.TypeUser = (UserType)request.TypeUser;
            entity.Active = (bool)request.Active;
            entity.DateOfBirth = request.DateOfBirth;
            entity.UpdatedAt = new DateUtc(DateTime.UtcNow);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
