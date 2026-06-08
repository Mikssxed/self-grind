namespace SelfGrind.Application.Community.Dtos;

public class LeaderboardDto
{
    public required string WeekStart { get; set; }
    public LeaderboardEntryDto[] Entries { get; set; } = [];
}
