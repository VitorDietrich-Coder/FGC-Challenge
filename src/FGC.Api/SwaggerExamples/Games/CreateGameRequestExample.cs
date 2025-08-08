using FGC.Application.Games.Commands.CreateGame;
using Swashbuckle.AspNetCore.Filters;

namespace FGC.Api.SwaggerExamples.Games
{
    
    public class CreateGameRequestExample : IExamplesProvider<CreateGameCommand>
    {
        public CreateGameCommand GetExamples()
        {
            return new CreateGameCommand
            {
                Name = "CS2",
                Price = 90.00M,
                Category = "FPS",
            };
        }
    }
}
