namespace SelfGrind.Domain.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string userName, string confirmationLink);
}
