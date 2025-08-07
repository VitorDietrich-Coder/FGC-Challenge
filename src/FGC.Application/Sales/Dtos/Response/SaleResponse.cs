using FGC.Domain.Entities.Sales;

namespace FGC.Application.Sales.Models.Response
{
    
    public record SaleResponse
    {
        public int SaleId { get; set; }
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }


        public static explicit operator SaleResponse(Sale sale)
        {
            return new SaleResponse
            {
                SaleId = sale.Id,
                Discount = sale.Discount.Value,
                StartDate = sale.StartDate.Value,
                ExpirationDate = sale.ExpirationDate.Value,
                Description = sale.Description
            };
        }
    }
}
