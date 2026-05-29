using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Analytics.Dtos;

public class StatGrowthSeriesDto
{
    public BaseAttribute Attribute { get; set; }
    public int[] Levels { get; set; } = [];
}
