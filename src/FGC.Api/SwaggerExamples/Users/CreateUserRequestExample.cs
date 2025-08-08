using FGC.Application.Users.Commands.CreateAdmin;
using FGC.Domain.Entities.Users.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace FGC.Api.SwaggerExamples.Users
{ 
    public class CreateUserAdminRequestExample : IExamplesProvider<CreateAdminCommand>
    {
        public CreateAdminCommand GetExamples()
        {
            return new CreateAdminCommand
            {
                Name = "User",
                Username = "User Gameplays",
                Email = "user@hotmail.com",
                Password = "Password123",
            };
        }
    }
}
