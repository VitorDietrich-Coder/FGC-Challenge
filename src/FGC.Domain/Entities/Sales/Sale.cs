using Abp.Domain.Entities;
using FGC.Domain.Core.Exceptions;
using IAggregateRoot = FGC.Domain.Core.IAggregateRoot;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;

namespace FGC.Domain.Entities.Sales
{
    public class Sale : Entity, IAggregateRoot
    {
        public Sale()
        {
        }

        public Sale(decimal discount, DateTime startDate, DateTime endDate, string description)
        {
            Discount = new CurrencyAmount(discount);
            StartDate = new DateUtc(startDate);
            ExpirationDate = new DateUtc(endDate);
            CreatedAt = new DateUtc(DateTime.UtcNow);
            Description = description;
        }

        public CurrencyAmount Discount { get; set; }
        public DateUtc StartDate { get; set; }
        public DateUtc ExpirationDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Game> Games { get; set; } = [];

        public void UpdateDiscount(decimal? discount, DateTime? endDate)
        {
            if (discount.HasValue)
                Discount = new CurrencyAmount(discount.Value);

            if (endDate.HasValue)
                ExpirationDate = new DateUtc(endDate.Value);
        }

        public void ValidatePeriod()
        {
            if (ExpirationDate <= StartDate.Value)
                throw new BusinessRulesException("Sale end date cannot be earlier than the start date.");
        }

        public bool IsExpired() => ExpirationDate < DateTime.UtcNow;

        public bool IsActive() => StartDate <= DateTime.UtcNow && ExpirationDate >= DateTime.UtcNow;

        public decimal GetDiscountPrice(decimal originalPrice)
        {
            return originalPrice * (1 - Discount.Value / 100);
        }
    }
}
