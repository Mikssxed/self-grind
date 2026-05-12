using Microsoft.AspNetCore.Identity;

namespace SelfGrind.Domain.Entities;

public class User : IdentityUser
{
    public List<TaskItem> TaskItems { get; set; } = [];
    public List<Habit> Habits { get; set; } = [];
    public List<UserStat> Stats { get; set; } = [];
    public int Level { get; set; } = 1;
    public int RequiredExp { get; set; } = 600;
    public int Exp { get; set; } = 0;
}