using FGC.Application.Auth.Commands;
using FGC.Application.UnitTests;
using FluentValidation.TestHelper;
using Xunit;

namespace FGC.UnitTests.Application.AuthTests.Commands
{
    public class LoginCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly LoginCommandValidator _validator;

        public LoginCommandValidatorTests()
        {
            _validator = new LoginCommandValidator();
        }

        [Fact]
        public async Task Should_PassValidation_When_ValidInput()
        {
            var command = new LoginCommand
            {
                Email = "valid@email.com",
                Password = "Strong@123"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Should_Fail_When_Email_IsEmpty()
        {
            var command = new LoginCommand
            {
                Email = "",
                Password = "Strong@123"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(c => c.Email)
                  .WithErrorMessage("Email is required.");
        }

        [Fact]
        public async Task Should_Fail_When_Email_IsInvalid()
        {
            var command = new LoginCommand
            {
                Email = "invalid-email",
                Password = "Strong@123"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(c => c.Email)
                  .WithErrorMessage("Email must be a valid email address.");
        }

        [Fact]
        public async Task Should_Fail_When_Password_IsEmpty()
        {
            var command = new LoginCommand
            {
                Email = "valid@email.com",
                Password = ""
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(c => c.Password)
                  .WithErrorMessage("Password is required.");
        }
  
    }
}
