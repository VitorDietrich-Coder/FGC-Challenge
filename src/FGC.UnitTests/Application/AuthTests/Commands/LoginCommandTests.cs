using FGC.Application.Auth.Commands;
using FGC.Domain.Core.Exceptions;
using FGC.Domain.Entities.Users;
using FGC.Domain.Entities.Users.ValueObjects;
using FluentAssertions;
using System.Security.Authentication;

namespace FGC.Application.UnitTests.Auth.Commands.Login;

public class LoginCommandTests : TestFixture
{
    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User
        {
            Email = new Email("adminteste@fiapgames.com"),
            Name = "user teste",
            Username = "user gameplays",
            Password = new Password("1GamesAdmin@"),
            Active = true,  
        };

        base.Context.Users.Add(user);
        await base.Context.SaveChangesAsync();

        var command = new LoginCommand
        {
            Email = "adminteste@fiapgames.com",
            Password = "1GamesAdmin@"
        };

        var handler = new LoginCommandHandler(base.Context, base.Configuration);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "naoexiste@fiapgames.com",
            Password = "123456"
        };

        var handler = new LoginCommandHandler(base.Context, base.Configuration);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenEmailIsInDifferentCase()
    {
        // Arrange
        var user = new User
        {
            Email = new Email("usercase@fiapgames.com"),
            Name = "Case Teste",
            Username = "usercase",
            Password = new Password("Senha123!"),
            Active = true
        };

        base.Context.Users.Add(user);
        await base.Context.SaveChangesAsync();

        var command = new LoginCommand
        {
            Email = "USERCASE@FIAPGAMES.COM", // case diferente
            Password = "Senha123!"
        };

        var handler = new LoginCommandHandler(base.Context, base.Configuration);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserIsInactive()
    {
        // Arrange
        var user = new User
        {
            Email = new Email("inativo@fiapgames.com"),
            Name = "Inativo Teste",
            Username = "inativo",
            Password = new Password("teste@123456"),
            Active = false
        };

        base.Context.Users.Add(user);
        await base.Context.SaveChangesAsync();

        var command = new LoginCommand
        {
            Email = "inativo@fiapgames.com",
            Password = "123456"
        };

        var handler = new LoginCommandHandler(base.Context, base.Configuration);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("Account is disabled.");
    }
}
