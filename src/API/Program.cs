using Cbs.Audit.AspNetCore; // AddHttpContextActor — an extension on CbsAuditBuilder, not part of the base DependencyInjection package
using Cbs.Audit.DependencyInjection; // AddCbsAudit, CbsAuditBuilder, AddEntityAuditing/AddLabelResolver/ValidateEntityActionsIn
using Cbs.Audit.Policy; // Mask
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

using Serilog;

using RECAMAS.Api.Middleware;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.DependencyInjection;
using RECAMAS.Infrastructure.Audit;
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

// Infrastructure (Settings, DB, Repos, HttpClients)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Cbs.Audit: call shape verified against the real Cbs.Audit source
// (AuditOptions.cs, ServiceCollectionExtensions.cs, CbsAuditBuilder.cs,
// AspNetCoreAuditExtensions.cs) — reads from AuditSettings instead of
// builder.Configuration[...] inline, to stay consistent with this project's
// .env conversion.
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
})
    .AddHttpContextActor()
    .AddEntityAuditing<ApplicationDbContext>()
    .AddLabelResolver<TCNProfileLabelResolver>()
    .ValidateEntityActionsIn(typeof(RECAMAS.Domain.Entities.TCNProfile.TCNProfile).Assembly);

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
