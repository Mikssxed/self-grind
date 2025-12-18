using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Infrastructure.Services.EmailModels;

namespace SelfGrind.Infrastructure.Services;

public class EmailService(
    ILogger<EmailService> logger,
    IWebHostEnvironment environment,
    IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendConfirmationEmailAsync(string email, string userName, string confirmationLink)
    {
        logger.LogInformation("Sending confirmation email to {Email}", email);

        var model = new ConfirmEmailModel
        {
            UserName = userName,
            Email = email,
            ConfirmationLink = confirmationLink
        };

        // Use ContentRootPath to locate the template (works for both API and tests)
        var templatePath = Path.Combine(environment.ContentRootPath, "EmailTemplates", "ConfirmEmail.cshtml");

        // Prepare the email with the template
        var emailBuilder = fluentEmail
            .To(email)
            .Subject("Confirm your email - SelfGrind")
            .UsingTemplateFromFile(templatePath, model);

        if (environment.IsDevelopment())
        {
            // In development save the email to a file instead of sending
            var emailsDir = Path.Combine(environment.ContentRootPath, "DevelopmentEmails");
            Directory.CreateDirectory(emailsDir);

            var fileName = $"ConfirmEmail_{email.Replace("@", "_").Replace(".", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            var filePath = Path.Combine(emailsDir, fileName);

            // Save the rendered HTML (Data.Body contains the rendered template)
            await File.WriteAllTextAsync(filePath, emailBuilder.Data.Body);

            logger.LogInformation("Development mode: Email saved to {FilePath}", filePath);
            logger.LogInformation("Open in browser: file:///{FilePath}", filePath.Replace("\\", "/"));
            logger.LogInformation("Confirmation email processed successfully for {Email}", email);
        }
        else
        {
            // In production send the email via SMTP
            var emailResponse = await emailBuilder.SendAsync();

            if (emailResponse.Successful)
            {
                logger.LogInformation("Confirmation email sent successfully to {Email}", email);
            }
            else
            {
                logger.LogError("Failed to send confirmation email to {Email}. Errors: {Errors}",
                    email, string.Join(", ", emailResponse.ErrorMessages));
            }
        }
    }
}
