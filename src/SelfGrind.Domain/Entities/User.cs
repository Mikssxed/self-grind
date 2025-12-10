using Microsoft.AspNetCore.Identity;

namespace SelfGrind.Domain.Entities;

public class User : IdentityUser
{
    public List<TaskItem> TaskItems { get; set; } = [];
    public string Username { get; set; } = string.Empty;
}