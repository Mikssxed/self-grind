using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetAchievements;

public class GetAchievementsQuery : IRequest<AchievementsDto>
{
}
