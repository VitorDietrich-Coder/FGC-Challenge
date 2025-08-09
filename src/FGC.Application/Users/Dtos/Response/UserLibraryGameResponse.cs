using FGC.Domain.Entities.Users;

namespace FGC.Application.Users.Models.Response
{
    public class UserLibraryGameResponse
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal BasePrice { get; set; }             
        public string BaseCurrency { get; set; } = null!; 
        public decimal FinalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string PurchaseCurrency { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
