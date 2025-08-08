﻿using FGC.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace FGC.Api.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected int GetUserId()
        {
            var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var user = userIdentity?.Claims.Where(c => c.Type == "id").FirstOrDefault();
            return user == null ? 0 : int.Parse(user.Value);
        }
    }

}
