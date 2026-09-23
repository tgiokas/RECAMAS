using ArsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArsApi.Infrastructure.Data;

public class ArsDbContext(DbContextOptions<ArsDbContext> options) : DbContext(options)
{
    public DbSet<ArsRecord> ArsRecords => Set<ArsRecord>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<ArsRecord>(e =>
        {
            e.ToTable("ars_records");
            e.HasKey(x => x.Id);
            e.Property(x => x.Arc).HasMaxLength(50).IsRequired();
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.PassportNo).HasMaxLength(50);
            e.Property(x => x.MdFileNo).HasMaxLength(50);
            e.Property(x => x.ResidencyDocumentNumber).HasMaxLength(50);
            e.Property(x => x.Address).HasMaxLength(300);
            e.Property(x => x.PhoneNo).HasMaxLength(30);
            e.Property(x => x.ReturnDecisionText).HasMaxLength(1000);
            e.Property(x => x.ArsFolderNumber).HasMaxLength(50);
            e.HasIndex(x => x.Arc);
            e.HasIndex(x => new { x.LastName, x.FirstName });
            e.HasIndex(x => x.ArsFolderNumber);
        });
    }
}
