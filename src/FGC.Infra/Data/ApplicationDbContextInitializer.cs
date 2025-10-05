using FGC.Domain.Common.ValueObjects;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FGC.Infra.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }

    public  void Initialise()
    {
        // Early development strategy
        //_context.Database.EnsureDeleted();
        //_context.Database.EnsureCreated();
        _context.Database.Migrate();

        // Late development strategy
        if (_context.Database.IsSqlServer())
        {
             _context.Database.Migrate();
        }
        else
        {
            _context.Database.EnsureCreated();
        }
    } 

    public void Seed()
    {
        SeedDeals();
        SeedUsers();
        SeedGames();
        SeedUserGamesLibrary();
    }

    private void SeedUsers()
    {
        if (_context.Users.Any())
            return;

        if (_context.Database.IsSqlServer())
        {
            using var transaction = _context.Database.BeginTransaction();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users ON");

            var users = GetUsers();
            _context.Users.AddRange(users);
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Users OFF");

            transaction.Commit();
        }
        else
        {
            var users = GetUsers();
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }

    private void SeedDeals()
    {
        if (_context.Deals.Any())
            return;

        if (_context.Database.IsSqlServer())
        {
            using var transaction = _context.Database.BeginTransaction();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Deals ON");

            var deals = GetDeals();
            _context.Deals.AddRange(deals);
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Deals OFF");

            transaction.Commit();
        }
        else
        {
            var deals = GetDeals();
            _context.Deals.AddRange(deals);
            _context.SaveChanges();
        }
    }
    private void SeedGames()
    {
        if (_context.Games.Any())
            return;

        if (_context.Database.IsSqlServer())
        {
            using var transaction = _context.Database.BeginTransaction();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Games ON");

            var games = GetGames();
            _context.Games.AddRange(games);
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Games OFF");

            transaction.Commit();
        }
        else
        {
            var games = GetGames();
            _context.Games.AddRange(games);
            _context.SaveChanges();
        }
    }

    private void SeedUserGamesLibrary()
    {
        if (_context.UserGamesLibrary.Any())
            return;

        if (_context.Database.IsSqlServer())
        {
            using var transaction = _context.Database.BeginTransaction();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.UserGamesLibrary ON");

            var userGames = GetUserGamesLibrary();
            _context.UserGamesLibrary.AddRange(userGames);
            _context.SaveChanges();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.UserGamesLibrary OFF");

            transaction.Commit();
        }
        else
        {
            var userGames = GetUserGamesLibrary();
            _context.UserGamesLibrary.AddRange(userGames);
            _context.SaveChanges();
        }
    }


    public static List<Deal> GetDeals()
    {
        return new List<Deal>
            {
                new Deal
				{
                    Id = 1,
                    StartDate = DateTime.SpecifyKind(new DateTime(2025, 06, 01), DateTimeKind.Utc),
                    ExpirationDate = DateTime.SpecifyKind(new DateTime(2026, 03, 01), DateTimeKind.Utc),
                    Description = "deal of Moba Games",
                    Discount = new CurrencyAmount(25 ,"BRL"),
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc),
                },
                new Deal
				{
                    Id = 2,
                    StartDate =  DateTime.SpecifyKind(new DateTime(2025, 06, 01), DateTimeKind.Utc),
                    ExpirationDate =  DateTime.SpecifyKind(new DateTime(2026, 04, 01), DateTimeKind.Utc),
                    Description = "deal of FPS Games",
                    Discount = new CurrencyAmount(15 ,"BRL"),
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc),
                },
                new Deal
				{
                    Id = 3,
                    StartDate = DateTime.SpecifyKind(new DateTime(2025, 07, 01), DateTimeKind.Utc),
                    ExpirationDate = DateTime.SpecifyKind(new DateTime(2026, 10, 01), DateTimeKind.Utc),
                    Description = "deal of RPG Games",
                    Discount = new CurrencyAmount(10 ,"BRL"),
                    CreatedAt  = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc),
                }
            };
    }

    public static List<Game> GetGames()
    {
        return new List<Game>
        {
            new Game { Id = 1, Name = "Cyberpunk 2077", Category = "Action RPG", Price = new CurrencyAmount(125.99M, "BRL"), CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc), },
            new Game { Id = 2, Name = "Horizon Zero Dawn", Category = "Action RPG", Price = new CurrencyAmount(130.99M, "BRL"),CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc), },
            new Game { Id = 3, Name = "Assassin's Creed Valhalla", Category = "Action-adventure", Price = new CurrencyAmount(150.40M, "BRL"), CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc), },
            new Game { Id = 4, Name = "Sekiro: Shadows Die Twice", Category = "Action RPG" , Price = new CurrencyAmount(35.99M, "BRL"), CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc)},
        };
    }

    public static List<User> GetUsers()
    {
        return new List<User>
        {
                new User
                {
                    Id = 1,
                    Name = "Admin New",
                    Username = "adminnew",
                    TypeUser = UserType.Admin,
                    Active = true,
                    Email = new Email("adminnew@fiapgames.com"),
                    Password = new Password("1GamesAdmin@"),
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc)

                },
                new User
                {
                    Id = 2,
                    Name = "User New",
                    Username = "usernew",
                    TypeUser = UserType.User,
                    Active = true,
                    Email = new Email("usernew@fiapgames.com"),
                    Password =  new Password("1GamesTeste@"),
                    CreatedAt = new DateTime(2025, 8, 2, 20, 0, 0, DateTimeKind.Utc)
                }
        };
    }
    public static List<UserGameLibrary> GetUserGamesLibrary()
    {
        return
        [
            new UserGameLibrary
            {
                Id = 1,
                UserId = 1,
                GameId = 1,
                DateOfPurchase = DateTime.SpecifyKind(new DateTime(2026, 04, 01), DateTimeKind.Utc),
                FinalPrice = new CurrencyAmount(20, "BRL"),
            },
            new UserGameLibrary
            {
                Id = 2,
                UserId = 2,
                GameId = 3,
                DateOfPurchase = DateTime.SpecifyKind(new DateTime(2026, 05, 01), DateTimeKind.Utc),
                FinalPrice = new CurrencyAmount(20, "BRL"),

            },
            new UserGameLibrary
            {
                Id = 3,
                UserId = 1,
                GameId = 4,
                DateOfPurchase = DateTime.SpecifyKind(new DateTime(2026, 06, 01), DateTimeKind.Utc),
                FinalPrice = new CurrencyAmount(20, "BRL"),
            },
        ];
    }

}
