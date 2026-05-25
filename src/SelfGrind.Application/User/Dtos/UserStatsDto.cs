namespace SelfGrind.Application.User.Dtos;

public class UserStatsDto
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public int RequiredExp { get; set; }
    public AttributeStatDto[] AttributeStats { get; set; } = [];
}
