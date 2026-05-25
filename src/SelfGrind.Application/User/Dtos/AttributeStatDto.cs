using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.User.Dtos;

public class AttributeStatDto
{
    public BaseAttribute Attribute { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int RequiredExp { get; set; }
}
