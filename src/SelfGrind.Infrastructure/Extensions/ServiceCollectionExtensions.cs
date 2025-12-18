using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Interfaces;
using SelfGrind.Domain.Repositories;
using SelfGrind.Infrastructure.Persistence;
using SelfGrind.Infrastructure.Repositories;
using SelfGrind.Infrastructure.Services;

namespace SelfGrind.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("SelfGrindDb");
        services.AddDbContext<SelfGrindDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SelfGrindDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
        });

        // FluentEmail configuration
        var smtpSettings = configuration.GetSection("SmtpSettings");
        var emailBuilder = services
            .AddFluentEmail(smtpSettings["SenderEmail"] ?? "noreply@selfgrind.com", smtpSettings["SenderName"] ?? "SelfGrind")
            .AddRazorRenderer();

        if (environment.IsDevelopment())
        {
            // W development nie wysyłamy prawdziwych emaili - tylko zapisujemy do pliku
            emailBuilder.AddSmtpSender("localhost", 25); // Fake sender - i tak EmailService zapisze do pliku
        }
        else
        {
            // W production używamy prawdziwego SMTP
            emailBuilder.AddSmtpSender(
                smtpSettings["Host"] ?? "localhost",
                int.Parse(smtpSettings["Port"] ?? "25"));
        }

        services.AddScoped<ITasksRepository, TasksRepository>();

        services.AddScoped<ITaskAuthorizationService, TaskAuthorizationService>();

        services.AddScoped<IEmailService, EmailService>();
    }
}