using ArrivalsApi.Application.Interfaces;
using ArrivalsApi.Application.Services;
using ArrivalsApi.Infrastructure.Data;
using ArrivalsApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ArrivalsDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IArrivalsRepository, ArrivalsRepository>();
builder.Services.AddScoped<ArrivalsService>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "CIT-RECAMAS Arrivals/Departures API", Version = "v1",
        Description = "Police DB — Arrivals/Departures §9.4 Implementation Study" });
    c.AddApiCredentialSecurity(builder.Configuration);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArrivalsDbContext>();
    for (int i = 0; i < 10; i++)
    {
        try { await db.Database.EnsureCreatedAsync(); break; }
        catch { Console.WriteLine($"[ARRIVALS] Waiting for PostgreSQL... {i+1}/10"); await Task.Delay(3000); }
    }
    var repo = scope.ServiceProvider.GetRequiredService<IArrivalsRepository>();
    if (!await repo.AnyAsync())
    {
        db.ArrivalRecords.AddRange(ArrivalsSeeder.GetSeedData());
        await db.SaveChangesAsync();
        Console.WriteLine("[ARRIVALS] Seeded 20 records.");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arrivals API v1"); c.RoutePrefix = string.Empty; });
app.UseApiCredentials();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
