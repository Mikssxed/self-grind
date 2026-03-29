using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Tasks.Dtos;
using SelfGrind.Application.User;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Tasks.Queries.GetTodayTaskItems;

public class GetTodayTaskItemsQueryHandler(
    ILogger<GetTodayTaskItemsQueryHandler> logger,
    IMapper mapper,
    ITasksRepository tasksRepository,
    IUserContext userContext)
    : IRequestHandler<GetTodayTaskItemsQuery, TodayTaskItemDto[]>
{
    public async Task<TodayTaskItemDto[]> Handle(GetTodayTaskItemsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var today = DateOnly.FromDateTime(DateTime.Now);
        logger.LogInformation("Fetching today's tasks for user {UserId} on {Date}", currentUser.Id, today);

        var occurrences = await tasksRepository.GetTodayTasksAsync(currentUser.Id, today, cancellationToken);
        return mapper.Map<TodayTaskItemDto[]>(occurrences);
    }
}
