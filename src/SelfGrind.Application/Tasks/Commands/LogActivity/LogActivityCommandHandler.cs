using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.LogActivity;

public class LogActivityCommandHandler(
    ILogger<LogActivityCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IItemsRepository itemsRepository,
    IUserItemsRepository userItemsRepository,
    IItemGrantingService itemGrantingService,
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

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user is null) throw new NotFoundException("user", currentUser.Id);

        var previousLevel = user.Level;
        statsService.AwardUserExp(user, request.Exp);

        var stat = user.Stats.FirstOrDefault(s => s.Attribute == request.Attribute);
        if (stat is not null) statsService.AwardStatExp(stat, request.Exp);

        await tasksRepository.SaveChangesAsync(cancellationToken);

        if (user.Level > previousLevel)
        {
            await GrantUnlockedItemsAsync(user.Id, user.Level, cancellationToken);
        }

        return taskId;
    }

    private async Task GrantUnlockedItemsAsync(string userId, int userLevel, CancellationToken cancellationToken)
    {
        var catalog = await itemsRepository.GetAllAsync(cancellationToken);
        var owned = await userItemsRepository.GetForUserAsync(userId, cancellationToken);
        var newGrants = itemGrantingService.CalculateNewlyGranted(userId, catalog, owned, userLevel);
        if (newGrants.Count > 0)
        {
            await userItemsRepository.AddRangeAsync(newGrants, cancellationToken);
        }
    }
}
