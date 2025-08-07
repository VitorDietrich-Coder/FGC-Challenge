using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Users.ValueObjects;

namespace FGC.UnitTests.Domain.ValueObjects
{
    public class EmailTest
    {
        [Fact]
        public void Email_ValidAddress_ShouldCreateSuccessfully()
        {
            // Arrange
            var validEmail = "example.user@fiap.com";

            // Act
            var email = new Email(validEmail);

            // Assert
            Assert.Equal(validEmail.ToLowerInvariant(), email.Address);
        }

        [Fact]
        public void Email_EmptyString_ShouldThrowBusinessRulesException()
        {
            // Arrange
            var emptyEmail = "";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Email(emptyEmail));
        }

        [Fact]
        public void Email_NullString_ShouldThrowBusinessRulesException()
        {
            // Arrange
            string? nullEmail = null;

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Email(nullEmail!));
        }

        [Fact]
        public void Email_TooLong_ShouldThrowBusinessRulesException()
        {
            // Arrange
            var longEmail = new string('a', 101) + "@fiap.com";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Email(longEmail));
        }

        [Fact]
        public void Email_InvalidFormat_ShouldThrowBusinessRulesException()
        {
            // Arrange
            var invalidEmail = "invalid-email-format";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Email(invalidEmail));
        }

        [Fact]
        public void Email_WithLeadingOrTrailingSpaces_ShouldBeTrimmed()
        {
            // Arrange
            var emailWithSpaces = "   student@fiap.com   ";

            // Act
            var email = new Email(emailWithSpaces);

            // Assert
            Assert.Equal("student@fiap.com", email.Address);
        }

        [Fact]
        public void Email_WithUppercaseLetters_ShouldBeNormalizedToLowercase()
        {
            // Arrange
            var mixedCaseEmail = "Student.User@FIAP.COM";

            // Act
            var email = new Email(mixedCaseEmail);

            // Assert
            Assert.Equal("student.user@fiap.com", email.Address);
        }
    }
}
