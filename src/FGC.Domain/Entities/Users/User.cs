using Abp.Domain.Entities;
using IAggregateRoot = FGC.Domain.Core.IAggregateRoot;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;

namespace FGC.Domain.Entities.Users
{
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            LibraryGames = new List<UserGameLibrary>();
            CreatedAt = DateTime.UtcNow;
        }

        public User(
            string name,
            string email,
            string password,
            UserType typeUser,
            bool active,
            string username,
            DateTime? updatedAt = null)
        {
            Name = name;
            Email = new Email(email);
            Password = new Password(password);
            TypeUser = typeUser;
            Active = active;
            Username = username;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = updatedAt;
            LibraryGames = new List<UserGameLibrary>();
        }

        public User(
           string name,
           string email,
           string password,
           UserType typeUser,
           bool active,
           string username,
           DateTime createdAt,
           DateTime? dateOfBirth = null,
           DateTime? updatedAt = null)

        {
            Name = name;
            Email = new Email(email);
            Password = new Password(password);
            TypeUser = typeUser;
            Active = active;
            Username = username;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            LibraryGames = new List<UserGameLibrary>();
        }

        public string Name { get; set; }
        public Email Email { get; set; }
        public Password Password { get; set; }
        public UserType TypeUser { get; set; }
        public bool Active { get; set; }

        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserGameLibrary> LibraryGames { get; set; }
    }
}
