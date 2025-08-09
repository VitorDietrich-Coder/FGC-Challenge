using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infra.Data.MapEntities
{
    public class UserGamesLibrary : IEntityTypeConfiguration<UserGameLibrary>
    {
        public void Configure(EntityTypeBuilder<UserGameLibrary> builder)
        {
            builder.ToTable("UserGamesLibrary");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.GameId)
                   .HasDatabaseName("GamesLibraryGameId");

            builder.HasIndex(x => x.UserId)
                   .HasDatabaseName("GamesLibraryUserId");

            builder.Property(x => x.DateOfPurchase)
                   .IsRequired();

            builder.OwnsOne(x => x.FinalPrice, price =>
            {
                price.Property(p => p.Value)
                     .HasColumnName("FinalPrice")
                     .IsRequired();

                price.Property(p => p.Currency)
                     .HasColumnName("FinalPriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });

            builder.HasOne(x => x.Game)
                   .WithMany(x => x.Libraries)
                   .HasForeignKey(x => x.GameId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.LibraryGames)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.CreatedAt)
                    .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .IsRequired(false);
        }
    }
}
