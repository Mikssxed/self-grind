using SelfGrind.Application.Extensions;
using SelfGrind.Domain.Entities;
using SelfGrind.Extensions;
using SelfGrind.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.AddPresentation();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();

