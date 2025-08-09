using FGC.Application.Auth.Commands;
using FGC.Domain.Entities.Users.ValueObjects;
using FGC.Domain.Entities.Users;
using FGC.Application.Auth.Models.Response;
using FGC.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
using Xunit;
using System.Threading.Tasks;
using FGC.Infra.Data;
using Microsoft.EntityFrameworkCore;
using FGC.Api.Controllers;
using FGC.Application.UnitTests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FGC.Api.UnitTests.Login
{
    public class AuthControllerTests : TestFixture
    {
        readonly AuthController _controller;
        readonly Mock<ISender> _mediatorMock;
    

        public AuthControllerTests()
        {
            // Criar Mock do Mediator
            _mediatorMock = new Mock<ISender>();

 

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_mediatorMock.Object) // registra o mock como ISender
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            _controller = new AuthController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange: cria usuário no banco de testes
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

            // Response esperado
            var expectedResponse = new LoginResponse
            {
                Token = "sample.jwt.token",
            };

            // Configura o Mock do mediator
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(expectedResponse);

            var command = new LoginCommand
            {
                Email = "adminteste@fiapgames.com",
                Password = "1GamesAdmin@"
            };

            // Act
            var result = await _controller.LoginAsync(command);

            // Assert
            Assert.Equal(result.Value.Token, expectedResponse.Token);
        }
    }
}
