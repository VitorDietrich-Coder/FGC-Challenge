using FGC.Domain.Entities.Users;

namespace FGC.Application.Users.Models.Response
{
    public record UserLibraryGameResponse
    {
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal BasePrice { get; set; }             
        public string BaseCurrency { get; set; } = null!; 
        public decimal FinalPrice { get; set; }
        public string PurchaseCurrency { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator UserLibraryGameResponse(UserGameLibrary libraryGame)
        {
            return new UserLibraryGameResponse
            {
                GameId = libraryGame.Game.Id,
                Name = libraryGame.Game.Name,
                Category = libraryGame.Game.Category,
                BasePrice = libraryGame.Game.Price.Value,          
                BaseCurrency = libraryGame.Game.Price.Currency,
                FinalPrice = libraryGame.FinalPrice.Value,
                PurchaseCurrency = libraryGame.FinalPrice.Currency,
                PurchaseDate = libraryGame.DateOfPurchase,
                CreatedAt = libraryGame.CreatedAt,
            };
        }
    }
}
