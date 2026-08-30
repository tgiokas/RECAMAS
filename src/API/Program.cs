using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

using Serilog;

using RECAMAS.Api.Middleware;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.DependencyInjection;
using RECAMAS.Infrastructure.DependencyInjection;
using RECAMAS.Infrastructure.Persistence;


Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Register health check services
builder.Services.AddHealthChecks();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
Log.Information("Configuration is starting...");

builder.Host.UseSerilog();

// Add Application services
builder.Services.AddApplicationServices();

// Infrastructure (Settings, DB, Repos, Kafka, HttpClients)
builder.Services.AddInfrastructureServices(builder.Configuration);

var keycloakSettings = KeycloakSettings.BindFromConfiguration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var databaseSettings = DatabaseSettings.BindFromConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Auto-migrate
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.Migrate();
Log.Information("Database migrations applied (if any).");
    
app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// --- DB migrations run automatically on startup, per CLAUDE.md convention ---
// (kept commented until the first module's migrations actually exist)
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     dbContext.Database.Migrate();
// }

app.Run();
