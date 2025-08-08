using FGC.Infra.Data.MapEntities;
using Microsoft.EntityFrameworkCore;
using FGC.Application.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FGC.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Users;


namespace FGC.Infra.Data;

public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
{
 
    public ApplicationDbContext(
        DbContextOptions options): base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<UserGameLibrary> UserGamesLibrary => Set<UserGameLibrary>();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder
            .LogTo(Console.WriteLine)
            .EnableDetailedErrors();
#endif

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new Users());
        builder.ApplyConfiguration(new Deals());
        builder.ApplyConfiguration(new Games());
        builder.ApplyConfiguration(new UserGamesLibrary());

        // Configuração direta do ValueConverter para UtcDate em deal
        var utcDateConverter = new ValueConverter<DateUtc, DateTime>(
            v => v.Value,
            v => new DateUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc))
        );

        var converter = new ValueConverter<DateTime, DateTime>(
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc),  // Ao salvar
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // Ao ler

        builder.Entity<UserGameLibrary>()
            .Property(e => e.DateOfPurchase)
            .HasConversion(utcDateConverter);

        builder.Entity<Deal>(entity =>
        {
            entity.Property(s => s.StartDate).HasConversion(utcDateConverter);
            entity.Property(s => s.ExpirationDate).HasConversion(utcDateConverter);
        });
    }

}

