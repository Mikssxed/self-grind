namespace SelfGrind.Application.User;

public record CurrentUser(string Id, string Email, IEnumerable<string> Roles, string username)
{
    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}