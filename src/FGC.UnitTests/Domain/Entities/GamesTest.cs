using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Games;
using FGC.Domain.Entities.Deals;
using FGC.Domain.Entities.Users;
using FGC.Infra.Data.MapEntities;

namespace FGC.UnitTests.Domain.Entities
{
    public class GamesTest
    {
        [Fact]
        public void ParameterlessConstructor_ShouldCreateInstance()
        {
            var game = new Game();

            Assert.NotNull(game);
            Assert.Null(game.Name);
            Assert.Null(game.Category);
            Assert.Equal(null, game.Price);
            Assert.Null(game.DealId);
            Assert.Null(game.Deal);
            Assert.Null(game.Libraries);
        }

        [Fact]
        public void Constructor_WithBasicParameters_ShouldSetProperties()
        {
            var game = new Game("Game1", "Action", 49.99M, null);

            Assert.Equal("Game1", game.Name);
            Assert.Equal("Action", game.Category);
            Assert.Equal(49.99M, game.Price.Value);
            Assert.Equal("BRL", game.Price.Currency);
            Assert.Null(game.DealId);
        }

        [Fact]
        public void Constructor_WithId_ShouldSetAllProperties()
        {
            var game = new Game(10, "Game With ID", "RPG", 59.99M, 5);

            Assert.Equal(10, game.Id);
            Assert.Equal("Game With ID", game.Name);
            Assert.Equal("RPG", game.Category);
            Assert.Equal(59.99M, game.Price.Value);            
            Assert.Equal(5, game.DealId);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-9999999999999999)]
        public void Constructor_ShouldThrowException_WhenPriceIsNegative(decimal price)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game("Game", "Action", price, null));

            Assert.Equal("The monetary amount must be greater than or equal to zero.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldAllow_WhenPriceIsZero()
        {
            var game = new Game("Free Game", "Indie", 0, null);

            Assert.Equal(0, game.Price.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenNameIsInvalid(string name)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game(name, "Action", 49.99M, null));

            Assert.Equal("Game name is required.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowException_WhenGenreIsInvalid(string genre)
        {
            var ex = Assert.Throws<BusinessRulesException>(() =>
                new Game("Game", genre, 49.99M, null));

            Assert.Equal("Category game is required.", ex.Message);
        }

        [Fact]
        public void AssignPromotion_ShouldSetPromotionId()
        {
            var game = new Game("Game", "Puzzle", 19.99M, null);
            game.UpdateDeal(5);

            Assert.Equal(5, game.DealId);
        }

        [Fact]
        public void Promotion_Property_ShouldWorkCorrectly()
        {
            var deal = new Deal(20, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "description");
            var game = new Game { Deal = deal };

            Assert.NotNull(game.Deal);
            Assert.Equal(20, game.Deal.Discount.Value);
        }

        [Fact]
        public void PromotionId_ShouldWork_WithoutPromotionObject()
        {
            var game = new Game { DealId = 5 };

            Assert.Equal(5, game.DealId);
            Assert.Null(game.Deal);
        }

        [Fact]
        public void Should_Handle_Null_Promotion()
        {
            var game = new Game { Deal = null };

            Assert.Null(game.Deal);
        }

        [Fact]
        public void Libraries_ShouldInitialize_WhenAssigned()
        {
            var game = new Game();
            game.Libraries = new List<UserGameLibrary> { new() };

            Assert.NotNull(game.Libraries);
            Assert.Single(game.Libraries);
        }

        [Fact]
        public void Libraries_ShouldBeNull_ByDefault()
        {
            var game = new Game();

            Assert.Null(game.Libraries);
        }

        [Fact]
        public void Should_Add_To_Libraries()
        {
            var game = new Game { Libraries = new List<UserGameLibrary>() };
            var libraryGame = new UserGameLibrary();

            game.Libraries.Add(libraryGame);

            Assert.Contains(libraryGame, game.Libraries);
        }

        [Fact]
        public void Constructor_ShouldHandle_MaxDoublePrice()
        {
            var game = new Game("Expensive Game", "AAA", decimal.MaxValue, null);

            Assert.Equal(decimal.MaxValue, game.Price.Value);
        }

        [Fact]
        public void Constructor_ShouldHandle_LongStrings()
        {
            var game = new Game(new string('A', 1000), new string('B', 500), 59.99M, null);

            Assert.Equal(1000, game.Name.Length);
            Assert.Equal(500, game.Category.Length);
        }

        [Fact]
        public void ValidatePrice_ShouldPass_ForValidValues()
        {
            var game1 = new Game("Game1", "Action", 0, null);
            var game2 = new Game("Game2", "Action", 10.5M, null);
            var game3 = new Game("Game3", "Action", decimal.MaxValue, null);

            Assert.Equal(0, game1.Price.Value);
            Assert.Equal(10.5M, game2.Price.Value);
            Assert.Equal(decimal.MaxValue, game3.Price.Value);
        }

        [Fact]
        public void Libraries_ShouldBeInitializedBeforeUse()
        {
            var game = new Game();

            Assert.Null(game.Libraries);

            game.Libraries = new List<UserGameLibrary>();

            Assert.NotNull(game.Libraries);
            Assert.Empty(game.Libraries);
        }

        [Fact]
        public void Promotion_ShouldUpdateCorrectly_WhenReassigned()
        {
            var game = new Game();
            var promotion1 = new Deal(10, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "description");
            var promotion2 = new Deal(20, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "description");

            game.Deal = promotion1;
            game.Deal = promotion2;

            Assert.Equal(20, game.Deal.Discount.Value);
        }

        [Fact]
        public void Libraries_Setter_ShouldAcceptNewCollection()
        {
            var game = new Game();
            var newLibraries = new List<UserGameLibrary> { new UserGameLibrary() };

            game.Libraries = newLibraries;

            Assert.NotNull(game.Libraries);
            Assert.Single(game.Libraries);
        }

        [Fact]
        public void Promotion_Setter_ShouldHandleReassignment()
        {
            var game = new Game();
            var deal1 = new Deal(15, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "description");
            var deal2 = new Deal(35, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "description");

            game.Deal = deal1;
            game.Deal = deal2;

            Assert.Equal(35, game.Deal.Discount.Value);
        }

        [Fact]
        public void CreateGame_ShouldSetPriceWithCurrency()
        {
            // Arrange
            var name = "Test Game";
            var category = "Action";
            var price = 99.99M;
            var dealId = (int?)null;

            // Act
            var game = new Game(name, category, price, dealId);

            // Assert
            Assert.Equal(name, game.Name);
            Assert.Equal(category, game.Category);
            Assert.Equal(price, game.Price.Value);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCurrencyIsInvalid()
        {
               HashSet<string> ValidCurrencies = new HashSet<string>
               {
                   "USD", // US Dollar
                   "EUR", // Euro
                   "BRL", // Brazilian Real
                   "JPY", // Japanese Yen
                   "GBP", // British Pound
                   "AUD", // Australian Dollar
                   "CAD", // Canadian Dollar
                   "CHF", // Swiss Franc
                   "CNY", // Chinese Yuan
                   "SEK", // Swedish Krona
                   "NZD"  // New Zealand Dollar
               };
               var ex = Assert.Throws<BusinessRulesException>(() =>
                        new Game("Game", "Action", 49.99M, null, "INVALID"));

            Assert.Equal($"Invalid currency: 'INVALID'. Supported currencies are: {string.Join(", ", ValidCurrencies)}.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldAllow_WhenCurrencyIsValid()
        {
            var game = new Game("Game", "Action", 49.99M, null, "USD");

            Assert.Equal(49.99M, game.Price.Value);
            Assert.Equal("USD", game.Price.Currency);
        }

        [Fact]
        public void CreateGame_ShouldSetPriceWithValidCurrency()
        {
            // Arrange
            var name = "Test Game";
            var genre = "Action";
            var price = 99.99M;
            var dealId = (int?)null;
            var currency = "USD";

            // Act
            var game = new Game(name, genre, price, dealId, currency);

            // Assert
            Assert.Equal(name, game.Name);
            Assert.Equal(genre, game.Category);
            Assert.Equal(price, game.Price.Value);
            Assert.Equal(currency, game.Price.Currency);
        }
    }
}
