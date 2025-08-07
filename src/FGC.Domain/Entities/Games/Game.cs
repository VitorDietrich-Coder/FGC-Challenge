using Abp.Domain.Entities;
using IAggregateRoot = FGC.Domain.Core.IAggregateRoot;
using FGC.Domain.Core.Exceptions;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Common;
using FGC.Domain.Entities.Sales;
using FGC.Domain.Entities.Users;

namespace FGC.Domain.Entities.Games
{
    public class Game : Entity, IAggregateRoot
    {
        public Game(int id, string name, string genre, decimal price, int? saleId, string currency = "BRL")
            : this(name, genre, price, saleId)
        {
            Id = id;
        }

        public Game(string name, string category, decimal price, int? saleId, string currency = "BRL")
        {
            ValidateName(name);
            ValidateCategory(category);

            Name = name;
            Category = category;
            Price = new CurrencyAmount(price, currency);
            SaleID = saleId;
            CreatedAt = new DateUtc(DateTime.UtcNow);
        }

        public Game() { }

        public string Name { get; set; }
        public string Category { get; set; }
        public CurrencyAmount Price { get; set; }
        public DateTime CreatedAt { get; set; }
 
        public int? SaleID { get; set; }

        public virtual Sale Sale { get; set; }
        public virtual ICollection<UserGameLibrary> Libraries { get; set; }

        public void UpdateSale(int saleId)
        {
            SaleID = saleId;
        }
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessRulesException("Game name is required.");
        }
        private void ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new BusinessRulesException("Category game is required.");
        }
    }
}
