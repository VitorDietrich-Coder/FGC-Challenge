using FGC.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FGC.Infra.Data.MapEntities
{
    public class Users : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.TypeUser)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(u => u.Active)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .IsRequired(false);

            builder.HasMany(u => u.LibraryGames)
                   .WithOne(lg => lg.User)
                   .HasForeignKey(lg => lg.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(u => u.Password, pw =>
            {
                pw.Property(p => p.Hash)
                  .HasColumnName("PasswordHash")
                  .IsRequired();
 
                pw.WithOwner();
            });

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(100);

                email.HasIndex(e => e.Address)
                     .IsUnique()
                     .HasDatabaseName("UsersEmail");
            });
        }
    }
}
