using FGC.Application.Deals.Commands.UpdateDeal;
using Swashbuckle.AspNetCore.Filters;


namespace FGC.Api.SwaggerExamples.Deals
{ 
    public class UpdatedealRequestExample : IExamplesProvider<UpdateDealCommand>
    {
        public UpdateDealCommand GetExamples()
        {
            return new UpdateDealCommand
            {
                Discount = 10,
                ExpirationDate = DateTime.UtcNow.AddDays(15),
                GameId = [1, 2, 3],
                Description = "Discount in FPS Games"
            };
        }
    }
}
