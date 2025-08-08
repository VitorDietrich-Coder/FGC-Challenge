using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Deals;

namespace FGC.Infra.Data.MapEntities
{
    public class Deals : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.ToTable("Deals");

            builder.HasKey(x => x.Id);

            builder.OwnsOne(p => p.Discount, discount =>
            {
                discount.Property(d => d.Value)
                        .HasColumnName("DiscountValue")
                        .IsRequired();

                discount.Property(d => d.Currency)
                        .HasColumnName("DiscountCurrency")
                        .IsRequired()
                        .HasMaxLength(3);
            });

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.StartDate)
                   .HasConversion(
                       v => v.Value,
                       v => new DateUtc(v))
                   .IsRequired();

            builder.Property(p => p.ExpirationDate)
                   .HasConversion(
                       v => v.Value,
                       v => new DateUtc(v))
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .IsRequired(false);
        }
    }
}
