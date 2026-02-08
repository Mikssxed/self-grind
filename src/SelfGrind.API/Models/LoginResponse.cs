namespace SelfGrind.Models;

public class LoginResponse
{
    public string TokenType { get; }
    public string AccessToken { get; }
    public int ExpiresIn { get; }
    public string RefreshToken { get; set; }
}