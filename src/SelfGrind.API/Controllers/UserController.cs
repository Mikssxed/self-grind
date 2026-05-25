using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.User.Dtos;
using SelfGrind.Application.User.Queries.GetUserStats;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserStatsDto>> GetUserStats()
    {
        var userStats = await mediator.Send(new GetUserStatsQuery());
        return Ok(userStats);
    }
}
