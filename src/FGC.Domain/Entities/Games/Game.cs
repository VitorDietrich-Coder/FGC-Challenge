using Abp.Domain.Entities;
using IAggregateRoot = FGC.Domain.Core.IAggregateRoot;
using FGC.Domain.Core.Exceptions;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Users;

namespace FGC.Domain.Entities.Games
{
    public class Game : Entity, IAggregateRoot
    {
        public Game(int id, string name, string genre, decimal price, int? dealId, string currency = "BRL")
            : this(name, genre, price, dealId)
        {
            Id = id;
        }

        public Game(string name, string category, decimal price, int? dealId, string currency = "BRL")
        {
            ValidateName(name);
            ValidateCategory(category);

            Name = name;
            Category = category;
            Price = new CurrencyAmount(price, currency);
            DealId = dealId;
            CreatedAt = DateTime.UtcNow;
        }

        public Game() { }

        public string Name { get; set; }
        public string Category { get; set; }
        public CurrencyAmount Price { get; set; }
        public DateTime CreatedAt { get; set; }
 
        public int? DealId { get; set; }

        public virtual Deal Deal { get; set; }
        public virtual ICollection<UserGameLibrary> Libraries { get; set; }

        public void UpdateDeal(int dealId)
        {
            DealId = dealId;
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
