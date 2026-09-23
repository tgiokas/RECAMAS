using CassApi.Application.Interfaces;
using CassApi.Application.Services;
using CassApi.Infrastructure.Data;
using CassApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Web;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CassDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ICassRepository, CassRepository>();
builder.Services.AddScoped<CassService>();
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "CIT-RECAMAS CASS API", Version = "v1",
        Description = "Cyprus Asylum Service — §9.3 Implementation Study" });
    c.AddApiCredentialSecurity(builder.Configuration);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CassDbContext>();
    for (int i = 0; i < 10; i++)
    {
        try { await db.Database.EnsureCreatedAsync(); break; }
        catch { Console.WriteLine($"[CASS] Waiting for PostgreSQL... {i+1}/10"); await Task.Delay(3000); }
    }
    var repo = scope.ServiceProvider.GetRequiredService<ICassRepository>();
    if (!await repo.AnyAsync())
    {
        db.CassRecords.AddRange(CassSeeder.GetSeedData());
        await db.SaveChangesAsync();
        Console.WriteLine("[CASS] Seeded 20 records.");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CASS API v1"); c.RoutePrefix = string.Empty; });
app.UseApiCredentials();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();
