using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using System.Diagnostics.CodeAnalysis;

namespace FGC.Application.Users.Models.Response
{
    
    public record UserResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public UserType Type { get; set; }
        public bool Active { get; set; }

        public static explicit  operator UserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email.Address,
                Type = user.TypeUser,
                Active = user.Active
            };
        }
    }   
}





