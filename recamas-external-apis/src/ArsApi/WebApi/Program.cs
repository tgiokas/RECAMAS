using ArsApi.Application.Interfaces;
using ArsApi.Application.Services;
using ArsApi.Infrastructure.Data;
using ArsApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ArsDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IArsRepository, ArsRepository>();
builder.Services.AddScoped<ArsService>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "CIT-RECAMAS ARS API", Version = "v1",
        Description = "Alien Registration System — §9.2 Implementation Study" });
    c.AddApiCredentialSecurity(builder.Configuration);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArsDbContext>();
    for (int i = 0; i < 10; i++)
    {
        try { await db.Database.EnsureCreatedAsync(); break; }
        catch { Console.WriteLine($"[ARS] Waiting for PostgreSQL... {i+1}/10"); await Task.Delay(3000); }
    }
    var repo = scope.ServiceProvider.GetRequiredService<IArsRepository>();
    if (!await repo.AnyAsync())
    {
        db.ArsRecords.AddRange(ArsSeeder.GetSeedData());
        await db.SaveChangesAsync();
        Console.WriteLine("[ARS] Seeded 20 records.");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ARS API v1"); c.RoutePrefix = string.Empty; });
app.UseApiCredentials();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
