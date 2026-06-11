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
        services.AddDbContext<SelfGrindDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<SelfGrindDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        });

        // Configure JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtService, JwtService>();

        // FluentEmail configuration
        var smtpSettings = configuration.GetSection("SmtpSettings");
        var emailBuilder = services
            .AddFluentEmail(smtpSettings["SenderEmail"] ?? "noreply@selfgrind.com", smtpSettings["SenderName"] ?? "SelfGrind")
            .AddRazorRenderer();

        if (environment.IsDevelopment())
        {
            // In development EmailService writes emails to a file, so this sender is never used
            emailBuilder.AddSmtpSender("localhost", 25);
        }
        else
        {
            emailBuilder.AddSmtpSender(
                smtpSettings["Host"] ?? "localhost",
                int.Parse(smtpSettings["Port"] ?? "25"));
        }

        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IHabitsRepository, HabitsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IEvolutionTiersRepository, EvolutionTiersRepository>();
        services.AddScoped<ISkillsRepository, SkillsRepository>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        services.AddScoped<IUserItemsRepository, UserItemsRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

        services.AddScoped<ITaskAuthorizationService, TaskAuthorizationService>();

        services.AddScoped<IEmailService, EmailService>();
    }
}