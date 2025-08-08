using FGC.Application.Deals.Commands.CreateDeal;
using Swashbuckle.AspNetCore.Filters;

namespace FGC.Api.SwaggerExamples.Deals
{
    
    public class CreateDealRequestExample : IExamplesProvider<CreateDealCommand>
    {
        public CreateDealCommand GetExamples()
        {
            return new CreateDealCommand
            {
                Discount = 25,
                ExpirationDate = DateTime.UtcNow.AddDays(15),
                GameId = [1, 2, 3],
                Description = "Discount in FPS Games"
            };
        }
    }
}
