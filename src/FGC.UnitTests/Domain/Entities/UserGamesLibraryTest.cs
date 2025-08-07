using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;

namespace FGC.Unit.Tests.Domain.Entities
{
    public class UserGamesLibraryTest
    {
        [Fact]
        public void LibraryGame_ShouldInitializeAndAssignPropertiesCorrectly()
        {
            // Arrange
            var id = 1;
            var userId = 10;
            var gameId = 20;
            var purchaseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var pricePaid = 59.99M;

            var mockGame = new FGC.Domain.Entities.Games.Game { Id = gameId, Name = "Mock Game" };
            var mockUser = new FGC.Domain.Entities.Users.User("Mock", "mock@email.com", "invalidHash@1salt", UserType.User, true, "nome do usuario")
            {
                Id = userId
            };

            // Act
            var entity = new UserGameLibrary(id, userId, gameId, purchaseDate, pricePaid)
            {
                Game = mockGame,
                User = mockUser
            };

            // Assert
            Assert.Equal(id, entity.Id);
            Assert.Equal(userId, entity.UserId);
            Assert.Equal(gameId, entity.GameId);
            Assert.Equal(purchaseDate, entity.DateOfPurchase);
            Assert.Equal(pricePaid, entity.FinalPrice.Value);
            Assert.Equal(mockGame, entity.Game);
            Assert.Equal(mockUser, entity.User);
        }

        [Fact]
        public void LibraryGame_DefaultConstructor_ShouldAllowManualPropertyAssignment()
        {
            // Arrange
            var id = 1;
            var userId = 2;
            var gameId = 3;
            var purchaseDate = DateTime.UtcNow;
            var pricePaid = 99.99M;

            // Act                       
            var entity = new UserGameLibrary(id, userId, gameId, purchaseDate, pricePaid)
            {
                Game = new FGC.Domain.Entities.Games.Game { Id = 3, Name = "Manual Game" },
                User = new FGC.Domain.Entities.Users.User("Manual", "manual@email.com", "invalidHash@1salt", UserType.User, true, "nome do usuario")
                {
                    Id = 2
                }
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
    }
}
