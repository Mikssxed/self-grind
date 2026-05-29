using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Analytics.Dtos;

public class LifeBalanceEntryDto
{
    public BaseAttribute Attribute { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int RequiredExp { get; set; }
    public double Score { get; set; }
}
