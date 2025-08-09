using FGC.Application.Users.Commands.CreateUser;
using FluentValidation.TestHelper;

namespace FGC.Application.UnitTests.UserTests.Commands.CreateUserTests
{
    public class CreateUserCommandValidatorTests : IClassFixture<TestFixture>
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserCommandValidatorTests()
        {
            _validator = new CreateUserCommandValidator();
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateUserCommand { Name = "" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Name_Is_Too_Long()
        {
            var command = new CreateUserCommand { Name = new string('a', 101) };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Name_Is_Valid()
        {
            var command = new CreateUserCommand { Name = "Valid User" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Too_Long()
        {
            var command = new CreateUserCommand { Username = new string('u', 101) };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }
 

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var command = new CreateUserCommand { Email = "" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Invalid()
        {
            var command = new CreateUserCommand { Email = "invalid-email" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Too_Long()
        {
            var command = new CreateUserCommand { Email = new string('e', 101) + "@test.com" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var command = new CreateUserCommand { Email = "user@example.com" };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Empty()
        {
            var command = new CreateUserCommand
            {
                Name = "Name",
                Email = "user@test.com",
                Password = ""
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Short()
        {
            var command = new CreateUserCommand
            {
                Name = "Name",
                Email = "user@test.com",
                Password = "A1@"
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Have_Error_When_Password_Is_Too_Long()
        {
            var command = new CreateUserCommand
            {
                Name = "Name",
                Email = "user@test.com",
                Password = new string('A', 101) + "1@"
            };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("Password123")]   // Missing special character
        [InlineData("password")]      // Missing uppercase + number + special
        [InlineData("PASSWORD@")]     // Missing lowercase + number
        [InlineData("Pass@word")]     // Missing number
        public async Task Should_Have_Error_When_Password_Does_Not_Match_Complexity(string password)
        {
            var command = new CreateUserCommand
            {
                Name = "Name",
                Email = "user@test.com",
                Password = password
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Password_Is_Valid()
        {
            var command = new CreateUserCommand
            {
                Name = "Name",
                Email = "user@test.com",
                Password = "Valid@123"
            };

            var result = await _validator.TestValidateAsync(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
