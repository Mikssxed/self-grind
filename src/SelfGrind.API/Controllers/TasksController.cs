using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Tasks.Commands.CreateTask;
using SelfGrind.Application.Tasks.Commands.DeleteTask;
using SelfGrind.Application.Tasks.Commands.LogActivity;
using SelfGrind.Application.Tasks.Commands.UpdateTask;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.Tasks.Queries.GetAllTaskItems;
using SelfGrind.Application.Tasks.Queries.GetTaskItemById;
using SelfGrind.Application.Tasks.Queries.GetContributionGrid;
using SelfGrind.Application.Tasks.Queries.GetDailySummary;
using SelfGrind.Application.Tasks.Queries.GetDayActivity;
using SelfGrind.Application.Tasks.Queries.GetTodayTaskItems;

namespace SelfGrind.Controllers;
[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("log-activity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> LogActivity([FromBody] LogActivityCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TaskItemDto[]>> GetAllTasks()
    {
        var tasks = await mediator.Send(new GetAllTaskItemsQuery());
        return Ok(tasks);
    }

    [HttpGet("today")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TodayTaskItemDto[]>> GetTodayTasks()
    {
        var tasks = await mediator.Send(new GetTodayTaskItemsQuery());
        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemDto>> GetTaskById([FromRoute] Guid id)
    {
        var task = await mediator.Send(new GetTaskItemByIdQuery(id));
        return Ok(task);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateTask([FromRoute] Guid id, [FromBody] UpdateTaskCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("daily-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DailySummaryDto>> GetDailySummary()
    {
        var summary = await mediator.Send(new GetDailySummaryQuery());
        return Ok(summary);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteTask([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteTaskCommand(id));
        return NoContent();
    }

    [HttpGet("contribution-grid")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ContributionGridDto>> GetContributionGrid([FromQuery] int year)
    {
        var grid = await mediator.Send(new GetContributionGridQuery { Year = year });
        return Ok(grid);
    }

    [HttpGet("day-activity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DayActivityDto>> GetDayActivity([FromQuery] DateOnly date)
    {
        var activity = await mediator.Send(new GetDayActivityQuery { Date = date });
        return Ok(activity);
    }
}
