using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;


namespace FGC.Application.Common;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Deal> Deals { get; }
    public DbSet<Game> Games { get; }
    public DbSet<UserGameLibrary> UserGamesLibrary { get; }

    // Alternative to defining all sets here
    // DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

