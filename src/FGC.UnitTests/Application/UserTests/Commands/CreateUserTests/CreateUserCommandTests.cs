using FGC.Application.Users.Commands.CreateUser;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Application.UnitTests;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.GamesTests.Commands.CreateUserTests
{
    public class CreateUserCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldCreateUser_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "Regular User",
                Username = "regularuser",
                Email = "user@example.com",
                Password = "User@1234"
            };

            var handler = new CreateUserCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Username.Should().Be(command.Username);
            result.Email.Should().Be(command.Email);
            result.Type.Should().Be(UserType.User);
            result.Active.Should().BeFalse();

            var createdUser = base.Context.Users.FirstOrDefault(u => u.Username == command.Username);
            createdUser.Should().NotBeNull();
            createdUser!.Name.Should().Be(command.Name);
            createdUser.Email.Address.Should().Be(command.Email);
            createdUser.TypeUser.Should().Be(UserType.User);
            createdUser.Active.Should().BeFalse();
        }
    }
}
