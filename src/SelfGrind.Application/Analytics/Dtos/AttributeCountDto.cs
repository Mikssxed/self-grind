using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Analytics.Dtos;

public class AttributeCountDto
{
    public BaseAttribute Attribute { get; set; }
    public int Count { get; set; }
}
