using SelfGrind.Application.Extensions;
using SelfGrind.Domain.Entities;
using SelfGrind.Extensions;
using SelfGrind.Infrastructure.Extensions;
using SelfGrind.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.AddPresentation();
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

