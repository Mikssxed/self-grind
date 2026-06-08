using MediatR;
using SelfGrind.Application.Community.Dtos;

namespace SelfGrind.Application.Community.Queries.GetLeaderboard;

public class GetLeaderboardQuery : IRequest<LeaderboardDto>
{
    public DateOnly? WeekStart { get; set; }
    public int Top { get; set; } = 10;
}
