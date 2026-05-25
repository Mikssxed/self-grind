using MediatR;
using SelfGrind.Application.User.Dtos;

namespace SelfGrind.Application.User.Queries.GetUserStats;

public class GetUserStatsQuery : IRequest<UserStatsDto>
{
}
