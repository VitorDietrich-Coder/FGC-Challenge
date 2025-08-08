using FGC.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace FGC.Infra.Data.MapEntities
{
    public class Games : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Name)
                .IsUnique()
                .HasDatabaseName("GamesName");

            builder.HasIndex(x => x.DealId)
                .HasDatabaseName("GamesDealId");

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.OwnsOne(x => x.Price, price =>
            {
                price.Property(p => p.Value)
                     .HasColumnName("Price")
                     .IsRequired();

                price.Property(p => p.Currency)
                     .HasColumnName("PriceCurrency")
                     .IsRequired()
                     .HasMaxLength(3);
            });


            builder.HasOne(x => x.Deal)
                   .WithMany(p => p.Games)
                   .HasForeignKey(x => x.DealId);

            builder.Property(u => u.CreatedAt)
                    .IsRequired();
        }
    }
}
