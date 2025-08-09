using FGC.Application.Users.Commands.UpdateUser;
using FGC.Domain.Entities.Users.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace FGC.Api.SwaggerExamples.Users
{
    
    public class UpdateUserRequestExample : IExamplesProvider<UpdateUserCommand>
    {
        public UpdateUserCommand GetExamples()
        {
            return new UpdateUserCommand
            {
                Name = "Maria Carie",
                Email = "maria.carie@hotmail.com",
                Password = "Password456!",
                TypeUser = UserType.Admin,
                Active = false,
            };
        }
    }
}
