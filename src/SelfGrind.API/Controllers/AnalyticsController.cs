using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Analytics.Dtos;
using SelfGrind.Application.Analytics.Queries.GetAchievements;
using SelfGrind.Application.Analytics.Queries.GetAnalyticsOverview;
using SelfGrind.Application.Analytics.Queries.GetLifeBalance;
using SelfGrind.Application.Analytics.Queries.GetStatGrowth;
using SelfGrind.Application.Analytics.Queries.GetWeeklyActivity;
using SelfGrind.Application.Analytics.Queries.GetXpPerWeek;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/analytics")]
[Authorize]
public class AnalyticsController(IMediator mediator) : ControllerBase
{
    [HttpGet("overview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AnalyticsOverviewDto>> GetOverview()
    {
        var overview = await mediator.Send(new GetAnalyticsOverviewQuery());
        return Ok(overview);
    }

    [HttpGet("weekly-activity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WeeklyActivityDto>> GetWeeklyActivity([FromQuery] DateOnly? weekStart)
    {
        var weekly = await mediator.Send(new GetWeeklyActivityQuery { WeekStart = weekStart });
        return Ok(weekly);
    }

    [HttpGet("xp-per-week")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<XpPerWeekDto>> GetXpPerWeek([FromQuery] int weeks = 6)
    {
        var data = await mediator.Send(new GetXpPerWeekQuery { Weeks = weeks });
        return Ok(data);
    }

    [HttpGet("stat-growth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<StatGrowthDto>> GetStatGrowth([FromQuery] int weeks = 6)
    {
        var data = await mediator.Send(new GetStatGrowthQuery { Weeks = weeks });
        return Ok(data);
    }

    [HttpGet("life-balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LifeBalanceDto>> GetLifeBalance()
    {
        var data = await mediator.Send(new GetLifeBalanceQuery());
        return Ok(data);
    }

    [HttpGet("achievements")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AchievementsDto>> GetAchievements()
    {
        var data = await mediator.Send(new GetAchievementsQuery());
        return Ok(data);
    }
}
