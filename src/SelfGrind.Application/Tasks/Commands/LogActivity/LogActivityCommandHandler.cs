using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Common;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.LogActivity;

public class LogActivityCommandHandler(
    ILogger<LogActivityCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IUserItemGranter userItemGranter,
    IStatsService statsService,
    IUserContext userContext) : IRequestHandler<LogActivityCommand, Guid>
{
    public async Task<Guid> Handle(LogActivityCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Logging activity '{title}' (+{exp} XP, {attribute}) for user {userId}",
            request.Title, request.Exp, request.Attribute, currentUser.Id);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayUtc = DateTime.UtcNow;

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Notes ?? string.Empty,
            Exp = request.Exp,
            Attribute = request.Attribute,
            UserId = currentUser.Id,
            IsCompleted = true,
            CreatedAt = todayUtc,
            UpdatedAt = todayUtc,
            Schedule = new TaskSchedule
            {
                StartDate = today,
                EndDate = today,
                RepetitionType = TaskRepetitionType.Once,
                RepeatInterval = 1,
            },
            Occurrences = new List<TaskOccurrence>
            {
                new()
                {
                    ScheduledDate = today,
                    CompletedDate = today,
                    Status = TaskOccurrenceStatus.Done,
                    CreatedAt = todayUtc,
                }
            }
        };

        var taskId = await tasksRepository.AddLoggedActivityAsync(task, cancellationToken);

        var user = await usersRepository.GetWithStatsOrThrowAsync(currentUser.Id, cancellationToken);

        var levelledUp = statsService.AwardTaskExp(user, request.Attribute, request.Exp);

        await tasksRepository.SaveChangesAsync(cancellationToken);

        if (levelledUp)
        {
            await userItemGranter.GrantUnlockedItemsAsync(user.Id, user.Level, cancellationToken);
        }

        return taskId;
    }
}
