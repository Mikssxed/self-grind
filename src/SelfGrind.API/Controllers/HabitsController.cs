using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Habits.Commands.CreateHabit;
using SelfGrind.Application.Habits.Commands.DeleteHabit;
using SelfGrind.Application.Habits.Commands.LogHabitEntry;
using SelfGrind.Application.Habits.Commands.UpdateHabit;
using SelfGrind.Application.Habits.Queries.GetHabits;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/habits")]
[Authorize]
public class HabitsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> CreateHabit([FromBody] CreateHabitCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<HabitDto[]>> GetAllAsync()
    {
        var habits = await mediator.Send(new GetHabitsQuery());
        return Ok(habits);
    }

    [HttpPost("{id}/entries")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> LogEntry([FromRoute] Guid id, [FromBody] LogHabitEntryCommand command)
    {
        command.HabitId = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateHabit([FromRoute] Guid id, [FromBody] UpdateHabitCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteHabit([FromRoute] Guid id)
    {
        await mediator.Send(new DeleteHabitCommand(id));
        return NoContent();
    }
}