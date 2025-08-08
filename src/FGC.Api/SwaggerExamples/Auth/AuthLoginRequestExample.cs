using FGC.Application.Auth.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Fiap.Api.SwaggerExamples.Auth
{
    
    public class AuthLoginRequestExample : IExamplesProvider<LoginCommand>
    {
        public LoginCommand GetExamples()
        {
            return new LoginCommand
            {
                Email = "adminnew@fiapgames.com",
                Password = "1GamesAdmin@"
            };
        }
    }
}
