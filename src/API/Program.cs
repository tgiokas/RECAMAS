using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RECAMAS.Api.Middleware;
using RECAMAS.Application.Configuration;
using RECAMAS.Application.DependencyInjection;
using RECAMAS.Infrastructure.DependencyInjection;
using RECAMAS.Infrastructure.Persistence;
using Serilog;

// --- Load .env into real process environment variables before anything reads config ---
// Only affects local (non-Docker) runs: in docker-compose, env_file already injects
// these directly. TraversePath() walks up from the working directory to find the
// repo-root .env regardless of whether the process is launched from src/API or the
// repo root; it silently no-ops when no .env file is found anywhere up the tree.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// --- Serilog, console + file (matches Authentication service's convention) ---
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// --- Layer registration, one call per layer, no hand-wiring here ---
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// --- JWT bearer auth against Keycloak, fronted by the reused Authentication service ---
// NOTE: Authority below assumes Keycloak is reachable directly for token validation
// (standard OIDC pattern) even though login itself goes through Authentication.
// RBAC (role -> allowed action) enforcement is a separate concern, added once the
// field-level/stage-level permission model is designed (Reports & Admin module).
var keycloakSettings = KeycloakSettings.BindFromConfiguration(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakSettings.Authority;
        options.Audience = keycloakSettings.Audience;
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseSettings = DatabaseSettings.BindFromConfiguration(builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(databaseSettings.ConnectionString, name: "postgres")
    // TODO once clients are finalized: add HTTP health checks pinging
    // Authentication, Storage, and a Kafka broker-connectivity check.
    ;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
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
