using CassApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CassApi.Infrastructure.Data;

public class CassDbContext(DbContextOptions<CassDbContext> options) : DbContext(options)
{
    public DbSet<CassRecord> CassRecords => Set<CassRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<CassRecord>(e =>
        {
            e.ToTable("cass_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Arc).HasMaxLength(50).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PassportNo).HasMaxLength(50);
            e.Property(x => x.CassFileNo).HasMaxLength(50);
            e.Property(x => x.AppealNumber).HasMaxLength(50);
            e.Property(x => x.ReturnDecisionText).HasMaxLength(1000);
            e.HasIndex(x => x.Arc);
        });
    }
}
