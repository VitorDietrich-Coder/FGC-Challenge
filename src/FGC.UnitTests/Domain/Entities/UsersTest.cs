using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;

namespace FGC.UnitTests.Domain.Entities
{
    public class UsersTest
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = 101;
            var userId = 501;
            var gameId = 301;
            var purchaseDate = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc);
            var finalPrice = 79.90M;

            var game = new FGC.Domain.Entities.Games.Game { Id = gameId, Name = "Cyber Trial" };
            var user = new FGC.Domain.Entities.Users.User("Alice", "alice@example.com", "hashedpass@1", UserType.User, true, "Alice Johnson")
            {
                Id = userId
            };

            // Act
            var entity = new UserGameLibrary(id, userId, gameId, purchaseDate, finalPrice)
            {
                Game = game,
                User = user
            };

            // Assert
            Assert.Equal(id, entity.Id);
            Assert.Equal(userId, entity.UserId);
            Assert.Equal(gameId, entity.GameId);
            Assert.Equal(purchaseDate, entity.DateOfPurchase);
            Assert.Equal(finalPrice, entity.FinalPrice.Value);
            Assert.Equal(game, entity.Game);
            Assert.Equal(user, entity.User);
        }

        [Fact]
        public void Constructor_ManualAssignment_ShouldAllowManualPropertySetting()
        {
            // Arrange
            var id = 202;
            var userId = 502;
            var gameId = 302;
            var purchaseDate = DateTime.UtcNow;
            var pricePaid = 49.99M;

            var game = new FGC.Domain.Entities.Games.Game { Id = gameId, Name = "Manual Warfare" };
            var user = new FGC.Domain.Entities.Users.User("Bob", "bob@example.com", "saltedHash@2", UserType.User, true, "Bob Smith")
            {
                Id = userId
            };

            // Act
            var entity = new UserGameLibrary(id, userId, gameId, purchaseDate, pricePaid)
            {
                Game = game,
                User = user
            };

            // Assert
            Assert.Equal(id, entity.Id);
            Assert.Equal(userId, entity.UserId);
            Assert.Equal(gameId, entity.GameId);
            Assert.Equal(purchaseDate, entity.DateOfPurchase);
            Assert.Equal(pricePaid, entity.FinalPrice.Value);
            Assert.NotNull(entity.Game);
            Assert.NotNull(entity.User);
        }

        [Fact]
        public void Constructor_FreeGame_ShouldAllowZeroPrice()
        {
            // Arrange
            var id = 303;
            var userId = 503;
            var gameId = 303;
            var purchaseDate = DateTime.UtcNow;
            var pricePaid = 0.0M;

            // Act
            var entity = new UserGameLibrary(id, userId, gameId, purchaseDate, pricePaid);

            // Assert
            Assert.Equal(0.0M, entity.FinalPrice.Value);
        }

        [Fact]
        public void Constructor_FuturePurchaseDate_ShouldStoreCorrectly()
        {
            // Arrange
            var id = 404;
            var userId = 504;
            var gameId = 304;
            var futureDate = DateTime.UtcNow.AddDays(5);
            var pricePaid = 59.90M;

            // Act
            var entity = new UserGameLibrary(id, userId, gameId, futureDate, pricePaid);

            // Assert
            Assert.Equal(futureDate, entity.DateOfPurchase);
        }
    }
}
