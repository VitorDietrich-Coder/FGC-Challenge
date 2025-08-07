using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Sales;

namespace FGC.Unit.Tests.Domain.Entities
{
    public class SalesTest
    {
        [Fact]
        public void Constructor_ValidData_ShouldCreateSaleSuccessfully()
        {
            // Arrange
            var discount = 15.5M;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(15);
            var description = "Back to School Sale";

            // Act
            var sale = new Sale(discount, startDate, endDate, description);

            // Assert
            Assert.Equal(discount, sale.Discount.Value);
            Assert.Equal(startDate, sale.StartDate);
            Assert.Equal(endDate, sale.ExpirationDate);
            Assert.Equal(description, sale.Description);
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenCurrentDateIsAfterEndDate()
        {
            // Arrange
            var sale = new Sale(20.0M, DateTime.UtcNow.AddDays(-20), DateTime.UtcNow.AddDays(-1), "Expired Sale");

            // Act
            var isExpired = sale.IsExpired();

            // Assert
            Assert.True(isExpired);
        }

        [Fact]
        public void IsActive_ShouldReturnTrue_WhenCurrentDateIsWithinStartAndEndDates()
        {
            // Arrange
            var sale = new Sale(10.0M, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(5), "Active Sale");

            // Act
            var isActive = sale.IsActive();

            // Assert
            Assert.True(isActive);
        }

        [Fact]
        public void IsActive_ShouldReturnFalse_WhenStartDateIsInFuture()
        {
            // Arrange
            var sale = new Sale(10.0M, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10), "Future Sale");

            // Act
            var isActive = sale.IsActive();

            // Assert
            Assert.False(isActive);
        }

        [Fact]
        public void ValidatePeriod_ShouldThrow_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            var sale = new Sale(5.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(-2), "Invalid Period Sale");

            // Act & Assert
            var ex = Assert.Throws<BusinessRulesException>(() => sale.ValidatePeriod());
            Assert.Equal("Sale end date cannot be earlier than the start date.", ex.Message);
        }

        [Fact]
        public void GetDiscountPrice_ShouldReturnCorrectDiscountedPrice()
        {
            // Arrange
            var sale = new Sale(25.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "Quarter Off");
            var originalPrice = 200.0M;

            // Act
            var discountedPrice = sale.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(150.0M, discountedPrice);
        }

        [Fact]
        public void UpdateDiscount_ShouldUpdateDiscountAndExpirationDate()
        {
            // Arrange
            var sale = new Sale(5.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), "Limited Offer");
            var newDiscount = 12.0M;
            var newEndDate = DateTime.UtcNow.AddDays(10);

            // Act
            sale.UpdateDiscount(newDiscount, newEndDate);

            // Assert
            Assert.Equal(newDiscount, sale.Discount.Value);
            Assert.Equal(newEndDate, sale.ExpirationDate);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDiscountIsNegative()
        {
            // Arrange
            var discount = -5.0M;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(5);
            var description = "Invalid Negative Discount";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Sale(discount, startDate, endDate, description));
        }

        [Fact]
        public void GetDiscountPrice_WithZeroPercentDiscount_ShouldReturnOriginalPrice()
        {
            // Arrange
            var sale = new Sale(0.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), "Zero Discount");
            var originalPrice = 100.0M;

            // Act
            var discountedPrice = sale.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(100.0M, discountedPrice);
        }

        [Fact]
        public void GetDiscountPrice_With100PercentDiscount_ShouldReturnZero()
        {
            // Arrange
            var sale = new Sale(100.0M, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), "Everything Free!");
            var originalPrice = 250.0M;

            // Act
            var discountedPrice = sale.GetDiscountPrice(originalPrice);

            // Assert
            Assert.Equal(0.0M, discountedPrice);
        }
    }
}
