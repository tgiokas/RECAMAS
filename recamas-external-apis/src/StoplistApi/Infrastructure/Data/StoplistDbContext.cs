using StoplistApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace StoplistApi.Infrastructure.Data;

public class StoplistDbContext(DbContextOptions<StoplistDbContext> options) : DbContext(options)
{
    public DbSet<StoplistRecord> StoplistRecords => Set<StoplistRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<StoplistRecord>(e =>
        {
            e.ToTable("stoplist_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Arc).HasMaxLength(50).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PassportNo).HasMaxLength(50);
            e.Property(x => x.UniqueEntryBanNumber).HasMaxLength(100);
            e.HasIndex(x => x.Arc);
            e.HasIndex(x => x.UniqueEntryBanNumber);
        });
    }
}
