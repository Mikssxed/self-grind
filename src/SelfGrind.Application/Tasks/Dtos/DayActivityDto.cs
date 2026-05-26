using SelfGrind.Domain.Constants;

namespace SelfGrind.Application.Tasks.Dtos;

public class DayActivityDto
{
    public DateOnly Date { get; set; }
    public DayActivityTaskDto[] Tasks { get; set; } = [];
    public int TotalXp { get; set; }
}

public class DayActivityTaskDto
{
    public string Title { get; set; } = string.Empty;
    public int Xp { get; set; }
    public BaseAttribute Attribute { get; set; }
}
