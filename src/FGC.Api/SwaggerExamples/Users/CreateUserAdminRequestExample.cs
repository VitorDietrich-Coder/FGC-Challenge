using FGC.Application.Users.Commands.CreateUser;
using Swashbuckle.AspNetCore.Filters;


namespace Fiap.Api.SwaggerExamples.Users
{
    
    public class CreateUserRequestExample : IExamplesProvider<CreateUserCommand>
    {
        public CreateUserCommand GetExamples()
        {
            return new CreateUserCommand
            {
                Name = "John Doe",
                Username = "username",
                Email = "john.doe@hotmail.com",
                Password = "Password123!",
            };
        }
    }
}
