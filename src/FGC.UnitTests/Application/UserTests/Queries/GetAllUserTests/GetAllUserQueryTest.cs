using FGC.Application.Users.Queries.GetAll;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.Enums;
using FGC.Domain.Entities.Users.ValueObjects;
using FGC.Application.UnitTests;
using FluentAssertions;
using Xunit;

namespace FGC.UnitTests.Application.UserTests.Queries.GetAllUserTests
{
    public class GetAllUserQueryTest : TestFixture
    {
        [Fact]
        public async Task Handle_ShouldReturnAllUsers_WhenUsersExist()
        {
            base.Context.Users.RemoveRange(base.Context.Users);
             await base.Context.SaveChangesAsync();

            // Arrange
            var user1 = new User
            {
                Name = "User One",
                Username = "userone",
                Email = new Email("user1@example.com"),
                Password = new Password("Password@123"),
                TypeUser = UserType.User,
                Active = true
            };

            var user2 = new User
            {
                Name = "User Two",
                Username = "usertwo",
                Email = new Email("user2@example.com"),
                Password = new Password("Password@123"),
                TypeUser = UserType.Admin,
                Active = false
            };

            base.Context.Users.AddRange(user1, user2);
            await base.Context.SaveChangesAsync();

            var query = new GetAllUsersQuery();
            var handler = new GetUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Any(u => u.Name == "User One").Should().BeTrue();
            result.Any(u => u.Name == "User Two").Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            base.Context.Users.RemoveRange(base.Context.Users);
            await base.Context.SaveChangesAsync();

            // Arrange
            var query = new GetAllUsersQuery();
            var handler = new GetUserQueryCommandHandler(base.Context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
