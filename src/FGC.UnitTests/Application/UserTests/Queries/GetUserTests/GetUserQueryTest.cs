using FGC.Application.Users.Queries.GetUser;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using FGC.Application.Users.Models.Response;
using FluentAssertions;
using Xunit;
using Ardalis.GuardClauses;
using FGC.Application.UnitTests;

namespace FGC.Application.UnitTests.UserTests.Queries.GetUserTests
{
    public class GetUserQueryTest : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Username = "testuser",
                Email = new Email("test@example.com"),
                Password = new Password("Valid@123"),
                TypeUser = UserType.User,
                Active = true
            };

            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var handler = new GetUserQueryCommandHandler(Context);
            var query = new GetUserQuery { Id = user.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(user.Id);
            result.Name.Should().Be(user.Name);
            result.Username.Should().Be(user.Username);
            result.Email.Should().Be(user.Email.Address);
            result.Type.Should().Be(user.TypeUser);
            result.Active.Should().Be(user.Active);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            base.Context.Users.RemoveRange(base.Context.Users);

            var handler = new GetUserQueryCommandHandler(Context);
            var query = new GetUserQuery { Id = 999 }; // nonexistent user

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*Users*");
        }
    }
}
