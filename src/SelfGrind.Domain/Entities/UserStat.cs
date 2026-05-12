using SelfGrind.Domain.Constants;

namespace SelfGrind.Domain.Entities;

public class UserStat
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public BaseAttribute Attribute { get; set; }
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;
    public int RequiredExp { get; set; } = 100;
    public required User User { get; set; }

}