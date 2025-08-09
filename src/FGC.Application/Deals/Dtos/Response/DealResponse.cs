using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Games;

namespace FGC.Application.Deals.Models.Response
{
    
    public record DealResponse
    {
        public int DealId { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }


        public static explicit operator DealResponse(Deal deal)
        {
            return new DealResponse
            {
                DealId = deal.Id,
                Discount = deal.Discount.Value,
                StartDate = deal.StartDate,
                ExpirationDate = deal.ExpirationDate,
                Description = deal.Description,
            };
        }
    }
}
