using Abp.Domain.Entities;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;

namespace FGC.Domain.Entities.Users
{
    public class UserGameLibrary : Entity
    {
        public UserGameLibrary()
        {            
        }

        public UserGameLibrary(int id, int userId, int gameId, DateTime purchaseDate, decimal pricePaid)
        {
            Id = id;
            UserId = userId;
            GameId = gameId;
            DateOfPurchase = purchaseDate;
            FinalPrice = new CurrencyAmount(pricePaid);
            CreatedAt = DateTime.Now;
        }

        public int GameId { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public CurrencyAmount FinalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
