using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

using DotNetEnv;
using Serilog;
using Cbs.Audit.AspNetCore;
using Cbs.Audit.DependencyInjection;
using Cbs.Audit.Policy;

using RECAMAS.Api.Middleware;
using RECAMAS.Application;
using RECAMAS.Application.Configuration;
using RECAMAS.Infrastructure;
using RECAMAS.Infrastructure.Audit;
using RECAMAS.Infrastructure.Database;

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

// Add Infrastructure services (Settings, DB, Repos, HttpClients)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Auditing 
var auditSettings = AuditSettings.BindFromConfiguration(builder.Configuration);

builder.Services.AddCbsAudit(o =>
{
    o.Project = "recamas";
    o.Env = auditSettings.Env ?? builder.Environment.EnvironmentName;
    o.ActionCatalogPath = Path.Combine(builder.Environment.ContentRootPath, "audit", "actions.yaml");
    o.Elasticsearch.Uri = auditSettings.ElasticsearchUri;
    o.Elasticsearch.Index = auditSettings.Index;
    o.Relay.Enabled = auditSettings.RelayEnabled;

    if (auditSettings.RelayMaxAttempts is int maxAttempts)
    {
        o.Relay.MaxAttempts = maxAttempts;
    }

    if (auditSettings.OutboxKeepDays is int keepDays)
    {
        o.Relay.KeepDelivered = TimeSpan.FromDays(keepDays);
    }

    var actorMask = auditSettings.ActorMask switch
    {
        "none" => (Mask?)null,
        "full" => Mask.Full,
        "initials" => Mask.Name,
        _ => Mask.Identity,
    };
    o.ActorPrivacy.DisplayName = actorMask;
    o.ActorPrivacy.Username = actorMask;
}).AddHttpContextActor()
.AddEntityAuditing<ApplicationDbContext>()
.AddLabelResolver<TCNProfileLabelResolver>()
.ValidateEntityActionsIn(typeof(RECAMAS.Domain.Entities.TCNProfile.TCNProfile).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as their string names instead of numeric values.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var shouldEnableSwagger = builder.Environment.IsDevelopment() || builder.Environment.IsStaging();
if (shouldEnableSwagger)
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (shouldEnableSwagger)
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
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
