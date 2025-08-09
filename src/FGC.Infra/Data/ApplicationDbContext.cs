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
    }

}

