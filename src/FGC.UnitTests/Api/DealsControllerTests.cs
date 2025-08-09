using FGC.Application.Deals.Commands.CreateDeal;
using FGC.Application.Deals.Commands.UpdateDeal;
using FGC.Application.Deals.Models.Response;
using FGC.Application.Deals.Queries.GetAlldeals;
using FGC.Application.Deals.Queries.Getdeals;
using FGC.Application.UnitTests;
using FGC.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FGC.Api.UnitTests.Login
{
    public class DealsControllerTests : TestFixture
    {
        private readonly DealsController _controller;
        private readonly Mock<ISender> _mediatorMock;

        public DealsControllerTests()
        {
            _mediatorMock = new Mock<ISender>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_mediatorMock.Object)
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            _controller = new DealsController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDeal_WhenIdIsValid()
        {
            // Arrange
            var dealId = 1;
            var expectedDeal = new DealResponse
            {
                DealId = dealId,
                Description = "Special Deal",
                Discount = 30
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetDealsByIdQuery>(q => q.Id == dealId), default))
                .ReturnsAsync(expectedDeal);

            // Act
            var result = await _controller.GetAsync(dealId);

            // Assert
            var value = Assert.IsType<ActionResult<DealResponse>>(result);
            Assert.Equal(expectedDeal.DealId, value.Value.DealId);
            Assert.Equal(expectedDeal.Description, value.Value.Description);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfDeals()
        {
            // Arrange
            var expectedDeals = new List<DealResponse>
            {
                new DealResponse { DealId = 1, Description = "Deal 1", Discount = 10 },
                new DealResponse { DealId = 2, Description = "Deal 2", Discount = 20 },
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllDealsQuery>(), default))
                .ReturnsAsync(expectedDeals);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var value = Assert.IsType<ActionResult<List<DealResponse>>>(result);
            Assert.Equal(2, value.Value.Count);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedDeal()
        {
            // Arrange
            var command = new CreateDealCommand
            {
                Description = "New Year Deal",
                Discount = 50
            };

            var expectedResponse = new DealResponse
            {
                DealId = 99,
                Description = command.Description,
                Discount = command.Discount
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateDealCommand>(c => c.Description == command.Description), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAsync(command);

            // Assert
            var value = Assert.IsType<ActionResult<DealResponse>>(result);
            Assert.Equal(expectedResponse.DealId, value.Value.DealId);
            Assert.Equal(expectedResponse.Description, value.Value.Description);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNoContent()
        {
            // Arrange
            var command = new UpdateDealCommand
            {
                Description = "Updated Deal",
                Discount = 25
            };

            var dealId = 10;

            _mediatorMock
                .Setup(m => m.Send(It.Is<UpdateDealCommand>(c => c.Id == dealId), default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateAsync(dealId, command);

            // Assert
            Assert.IsType<ActionResult<Unit>>(result);
        }
    }
}
