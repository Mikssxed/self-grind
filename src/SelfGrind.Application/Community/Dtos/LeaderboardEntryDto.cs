namespace SelfGrind.Application.Community.Dtos;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public required string Title { get; set; }
    public int Xp { get; set; }
    public bool IsCurrentUser { get; set; }
    public required string AvatarEmoji { get; set; }
}
