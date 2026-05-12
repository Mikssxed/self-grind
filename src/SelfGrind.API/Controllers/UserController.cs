using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IMediator mediator): ControllerBase
{
    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<UserStatsDto>> GetUserStats()
    // {
    //     var userStats = await mediator.Send(new GetUserStatsQuery());
    //     return Ok(userStats);
    // }
}