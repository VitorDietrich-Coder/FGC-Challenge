using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Users.ValueObjects;

namespace FGC.UnitTests.Domain.ValueObjects
{
    public class PasswordTest
    {
        [Fact]
        public void Password_ValidPlainText_ShouldGenerateHash()
        {
            // Arrange
            var plainPassword = "StrongPass@2025";

            // Act
            var password = new Password(plainPassword);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(password.Hash));
        }

        [Fact]
        public void Password_ValidPlainText_HashShouldStartWithBcryptPrefix()
        {
            // Arrange
            var password = new Password("Secure@123");

            // Assert
            Assert.StartsWith("$2b$", password.Hash);
        }

        [Fact]
        public void Password_ValidPassword_ShouldPassChallenge()
        {
            // Arrange
            var plainPassword = "User#Pass2025";
            var password = new Password(plainPassword);

            // Act
            var isValid = password.Challenge(plainPassword);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Password_WrongPassword_ShouldFailChallenge()
        {
            // Arrange
            var password = new Password("@CorrectPassword99");

            // Act
            var isValid = password.Challenge("@WrongPassword99");

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Password_SameInput_ShouldGenerateDifferentHashes()
        {
            // Arrange
            var input = "Repeatable@Password";

            // Act
            var password1 = new Password(input);
            var password2 = new Password(input);

            // Assert
            Assert.NotEqual(password1.Hash, password2.Hash); // due to random salt
        }

        [Fact]
        public void Password_FromExistingHash_ShouldBeConsistent()
        {
            // Arrange
            var original = new Password("Persistent@2024");
            var loaded = new Password(original.Hash);

            // Assert
            Assert.Equal(original.Hash, loaded.Hash);
        }

        [Fact]
        public void Password_GetAtomicValues_ShouldEnsureEqualityByHash()
        {
            // Arrange
            var password1 = new Password("Equality@Test123");
            var password2 = new Password(password1.Hash);

            // Act & Assert
            Assert.Equal(password1, password2);
        }

        [Fact]
        public void Password_TooShort_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var weakPassword = "aB1!";

            // Act & Assert
            Assert.Throws<BusinessRulesException>(() => new Password(weakPassword));
        }

        [Fact]
        public void Password_NullValue_ShouldThrowArgumentException()
        {
            // Arrange
            string? nullPassword = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Password(nullPassword!));
        }

        [Fact]
        public void Password_EmptyValue_ShouldThrowArgumentException()
        {
            // Arrange
            var emptyPassword = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Password(emptyPassword));
        }

        [Fact]
        public void Password_InvalidHashFormat_ShouldFailChallenge()
        {
            // Arrange
            var invalidHash = "invalid-hash-format";
            var password = new Password(invalidHash);

            // Act
            var result = password.Challenge("Any@Password123");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Password_DifferentInputs_ShouldProduceDifferentHashes()
        {
            // Arrange
            var password1 = new Password("PasswordOne@123");
            var password2 = new Password("PasswordTwo@123");

            // Assert
            Assert.NotEqual(password1.Hash, password2.Hash);
        }
    }
}
