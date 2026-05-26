using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetDayActivity;

public class GetDayActivityQueryHandler(
    ILogger<GetDayActivityQueryHandler> logger,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetDayActivityQuery, DayActivityDto>
{
    public async Task<DayActivityDto> Handle(GetDayActivityQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("Handling GetDayActivityQuery for user {UserId}, date {Date}", currentUser.Id, request.Date);

        var occurrences = await tasksRepository.GetCompletedOccurrencesForDateAsync(currentUser.Id, request.Date, cancellationToken);

        var tasks = occurrences.Select(o => new DayActivityTaskDto
        {
            Title = o.TaskItem.Title,
            Xp = o.TaskItem.Exp,
            Attribute = o.TaskItem.Attribute
        }).ToArray();

        return new DayActivityDto
        {
            Date = request.Date,
            Tasks = tasks,
            TotalXp = tasks.Sum(t => t.Xp)
        };
    }
}
