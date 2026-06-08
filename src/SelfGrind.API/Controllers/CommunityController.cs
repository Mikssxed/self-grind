using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Community.Dtos;
using SelfGrind.Application.Community.Queries.GetLeaderboard;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/community")]
[Authorize]
public class CommunityController(IMediator mediator) : ControllerBase
{
    [HttpGet("leaderboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LeaderboardDto>> GetLeaderboard(
        [FromQuery] DateOnly? weekStart,
        [FromQuery] int top = 10)
    {
        var leaderboard = await mediator.Send(new GetLeaderboardQuery
        {
            WeekStart = weekStart,
            Top = top,
        });
        return Ok(leaderboard);
    }
}
