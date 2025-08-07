using Abp.Domain.Entities;
using IAggregateRoot = FGC.Domain.Core.IAggregateRoot;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;

namespace FGC.Domain.Entities.Users
{
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            LibraryGames = new List<UserGameLibrary>();
            CreatedAt = new DateUtc(DateTime.UtcNow);
        }

        public User(
            string name,
            string email,
            string password,
            UserType typeUser,
            bool active,
            string username,
            DateTime? dateOfBirth = null,
            DateTime? updatedAt = null)
        {
            Name = name;
            Email = new Email(email);
            Password = new Password(password);
            TypeUser = typeUser;
            Active = active;
            Username = username;
            DateOfBirth = dateOfBirth;
            CreatedAt = new DateUtc(DateTime.UtcNow);
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
            DateOfBirth = dateOfBirth;
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
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UserGameLibrary> LibraryGames { get; set; }

        public static User CreateByAdmin(
            string name,
            string email,
            string password,
            UserType typeUser,
            bool active,
            string username,
            DateTime? dateOfBirth = null)
        {
            return new User(name, email, password, typeUser, active, username, dateOfBirth);
        }

        public static User CreateByPublic(
            string name,
            string email,
            string password,
            string username)
        {
            return new User(name, email, password, UserType.User, true, username);
        }
    }
}
