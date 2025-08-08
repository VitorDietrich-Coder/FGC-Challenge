using FGC.Api.Swagger.Attributes;
using FGC.Application.Common;
using FGC.Application.Deals.Commands.CreateDeal;
using FGC.Application.Deals.Commands.UpdateDeal;
using FGC.Application.Deals.Models.Response;
using FGC.Application.Deals.Queries.GetAlldeals;
using FGC.Application.Deals.Queries.Getdeals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FGC.Api.Controllers
{
    /// <summary>
    /// Controller for administrators to manage game deals.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DealsController : ApiControllerBase
    {
        /// <summary>
        /// Retrieves a deal by its ID.
        /// </summary>
        /// <param name="id">deal ID (must be greater than 0).</param>
        /// <returns>Returns the deal details if found.</returns>
        [HttpGet("{id:int:min(1)}")]
        [SwaggerOperation(
            Summary = "Retrieve a deal by ID.",
            Description = "Fetches deal details using the given ID. Returns 404 if not found, 400 for invalid input, and 401/403 for unauthorized access."
        )]
        [SwaggerResponseProfile("deal.Get")]
        [ProducesResponseType(typeof(SuccessResponse<DealResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DealResponse>> GetAsync(int id)
        {
            return await Mediator.Send(new GetDealsByIdQuery { Id = id });

        }

        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a deals",
          Description = "Fetches deal details using the given ID. Returns 404 if not found, 400 for invalid input, and 401/403 for unauthorized access."
         )]
        [SwaggerResponseProfile("deal.Get")]
        public async Task<ActionResult<List<DealResponse>>> GetAll()
        {
            return await Mediator.Send(new GetAllDealsQuery());
        }

        /// <summary>
        /// Creates a new deal for one or more games.
        /// </summary>
        /// <param name="request">Details of the deal to create.</param>
        /// <returns>Returns the created deal data.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new deal.",
            Description = "Creates a deal with discount, start and end dates. Optionally associates one or more games. Returns the created deal or validation errors."
        )]
        [SwaggerResponseProfile("deal.Create")]
        [ProducesResponseType(typeof(SuccessResponse<DealResponse>), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<DealResponse>> CreateAsync([FromBody] CreateDealCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Updates an existing deal and its associated games.
        /// </summary>
        /// <param name="id">ID of the deal to update (must be greater than 0).</param>
        /// <param name="request">Updated deal details.</param>
        /// <returns>Returns no content on success.</returns>
        [HttpPut("{id:int:min(1)}")]
        [SwaggerOperation(
            Summary = "Update a deal.",
            Description = "Modifies an existing deal's details and associated games. Returns 204 on success, 404 if not found, or validation errors."
        )]
        [SwaggerResponseProfile("deal.Update")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<Unit>> UpdateAsync(int id, [FromBody] UpdateDealCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
    }
}
