using FGC.Domain.Users.Entities;

namespace FGC.Application.Users.Models.Response
{
    
    public record UserLibraryGameResponse
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public string OriginalCurrency { get; set; } = null!;
        public decimal PricePaid { get; set; }
        public string PurchaseCurrency { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }

        public static explicit operator UserLibraryGameResponse(UserGameLibrary libraryGame)
        {
            return new UserLibraryGameResponse
            {
                GameId = libraryGame.Game.Id,
                Name = libraryGame.Game.Name,
                Category = libraryGame.Game.Category,
                OriginalPrice = libraryGame.Game.Price.Value,
                OriginalCurrency = libraryGame.Game.Price.Currency,
                PricePaid = libraryGame.PricePaid.Value,
                PurchaseCurrency = libraryGame.PricePaid.Currency,
                PurchaseDate = libraryGame.DateOfPurchase
            };
        }
    }
}
