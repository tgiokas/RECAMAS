using ArrivalsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArrivalsApi.Infrastructure.Data;

public class ArrivalsDbContext(DbContextOptions<ArrivalsDbContext> options) : DbContext(options)
{
    public DbSet<ArrivalRecord> ArrivalRecords => Set<ArrivalRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<ArrivalRecord>(e =>
        {
            e.ToTable("arrival_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Arc).HasMaxLength(50).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PassportNo).HasMaxLength(50);
            e.HasIndex(x => x.Arc);
            e.HasIndex(x => new { x.Arc, x.MovementDate });
        });
    }
}
