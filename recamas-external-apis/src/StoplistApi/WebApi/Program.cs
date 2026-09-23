using StoplistApi.Application.Interfaces;
using StoplistApi.Application.Services;
using StoplistApi.Infrastructure.Data;
using StoplistApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StoplistDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IStoplistRepository, StoplistRepository>();
builder.Services.AddScoped<StoplistService>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "CIT-RECAMAS Stoplist API", Version = "v1",
        Description = "Police DB — Entry-ban register §9.5 Implementation Study" });
    c.AddApiCredentialSecurity(builder.Configuration);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoplistDbContext>();
    for (int i = 0; i < 10; i++)
    {
        try { await db.Database.EnsureCreatedAsync(); break; }
        catch { Console.WriteLine($"[STOPLIST] Waiting for PostgreSQL... {i+1}/10"); await Task.Delay(3000); }
    }
    var repo = scope.ServiceProvider.GetRequiredService<IStoplistRepository>();
    if (!await repo.AnyAsync())
    {
        db.StoplistRecords.AddRange(StoplistSeeder.GetSeedData());
        await db.SaveChangesAsync();
        Console.WriteLine("[STOPLIST] Seeded 20 records.");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stoplist API v1"); c.RoutePrefix = string.Empty; });
app.UseApiCredentials();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
