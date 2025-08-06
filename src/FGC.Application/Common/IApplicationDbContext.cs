using FGC.Domain.GameAggregate;
using FGC.Domain.SaleAggregate;
using FGC.Domain.Users;
using FGC.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;


namespace FGC.Application.Common;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Sale> Sales { get; }
    public DbSet<Game> Games { get; }
    public DbSet<UserGameLibrary> UserGamesLibrary { get; }

    // Alternative to defining all sets here
    // DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

