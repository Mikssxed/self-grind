using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Stats.Services;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Commands.CompleteTaskOccurence;

public class CompleteTaskOccurenceCommandHandler(
    ILogger<CompleteTaskOccurenceCommandHandler> logger,
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IItemsRepository itemsRepository,
    IUserItemsRepository userItemsRepository,
    IItemGrantingService itemGrantingService,
    IStatsService statsService,
    IUserContext userContext) : IRequestHandler<CompleteTaskOccurenceCommand, Guid>
{
    public async Task<Guid> Handle(CompleteTaskOccurenceCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling CompleteTaskOccurenceCommand for TaskOccurrenceId: {taskOccurrenceId}", request.Id);

        var taskOccurence = await tasksRepository.GetTaskOccurenceById(currentUser.Id, request.Id, cancellationToken);

        if (taskOccurence == null)
        {
            throw new NotFoundException("task occurrence", request.Id.ToString());
        }

        if (taskOccurence.Status == TaskOccurrenceStatus.Done)
        {
            return taskOccurence.Id;
        }

        taskOccurence.Status = TaskOccurrenceStatus.Done;
        taskOccurence.CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("user", currentUser.Id);
        }

        var exp = taskOccurence.TaskItem.Exp;
        var previousLevel = user.Level;
        statsService.AwardUserExp(user, exp);

        var stat = user.Stats.FirstOrDefault(s => s.Attribute == taskOccurence.TaskItem.Attribute);
        if (stat != null)
        {
            statsService.AwardStatExp(stat, exp);
        }

        await tasksRepository.SaveChangesAsync(cancellationToken);

        if (user.Level > previousLevel)
        {
            await GrantUnlockedItemsAsync(user.Id, user.Level, cancellationToken);
        }

        return taskOccurence.Id;
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
