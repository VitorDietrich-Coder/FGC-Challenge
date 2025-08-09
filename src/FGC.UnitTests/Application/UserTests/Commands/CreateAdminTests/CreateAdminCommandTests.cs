using FGC.Application.Users.Commands.CreateAdmin;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Application.UnitTests;
using FluentAssertions;
using Xunit;

namespace FGC.Application.UnitTests.GamesTests.Commands.CreateAdminTests
{
    public class CreateAdminCommandTests : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldCreateAdmin_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateAdminCommand
            {
                Name = "Admin User",
                Username = "adminuser",
                Email = "admin@examples.com",
                Password = "Admin@1234"
            };

            var handler = new CreateAdminCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Username.Should().Be(command.Username);
            result.Email.Should().Be(command.Email);
            result.Type.Should().Be(UserType.Admin);
            result.Active.Should().BeTrue();

            var createdUser = base.Context.Users.FirstOrDefault(u => u.Username == command.Username);
            createdUser.Should().NotBeNull();
            createdUser!.Name.Should().Be(command.Name);
            createdUser.Email.Address.Should().Be(command.Email);
            createdUser.TypeUser.Should().Be(UserType.Admin);
            createdUser.Active.Should().BeTrue();
        }
    }
}
