namespace SelfGrind.Infrastructure.Services.EmailModels;

public class ConfirmEmailModel
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string ConfirmationLink { get; set; }
}
