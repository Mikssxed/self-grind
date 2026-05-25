using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.User.Dtos;
using SelfGrind.Domain.Exceptions;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.User.Queries.GetUserStats;

public class GetUserStatsQueryHandler(
    ILogger<GetUserStatsQueryHandler> logger,
    IMapper mapper,
    IUsersRepository usersRepository,
    IUserContext userContext)
    : IRequestHandler<GetUserStatsQuery, UserStatsDto>
{
    public async Task<UserStatsDto> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetUserStatsQuery for user {userId}", currentUser.Id);

        var user = await usersRepository.GetWithStatsAsync(currentUser.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("user", currentUser.Id);
        }

        var dto = mapper.Map<UserStatsDto>(user);
        dto.AttributeStats = dto.AttributeStats
            .OrderBy(s => (int)s.Attribute)
            .ToArray();

        return dto;
    }
}
