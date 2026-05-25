using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelfGrind.Application.Extensions;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;
using SelfGrind.Extensions;
using SelfGrind.Infrastructure.Extensions;
using SelfGrind.Infrastructure.Persistence;
using SelfGrind.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.AddPresentation();
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Automatically apply database migrations on startup (skip if no connection string)
var connectionString = builder.Configuration.GetConnectionString("SelfGrindDb");
if (!string.IsNullOrEmpty(connectionString))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SelfGrindDbContext>();
    dbContext.Database.Migrate();

    if (app.Environment.IsDevelopment())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var usersRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();

        const string devEmail = "dev@selfgrind.local";
        if (await userManager.FindByEmailAsync(devEmail) is null)
        {
            var devUser = new User { UserName = "dev", Email = devEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(devUser, "Dev123!Pass");
            if (result.Succeeded)
            {
                await usersRepository.SeedStatsAsync(devUser.Id);
            }
        }
    }
}

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

