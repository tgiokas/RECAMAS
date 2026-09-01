using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseEntity = RECAMAS.Domain.Entities.Case.Case;

namespace RECAMAS.Infrastructure.Database.Configurations.Case;

public class CaseConfiguration : IEntityTypeConfiguration<CaseEntity>
{
    public void Configure(EntityTypeBuilder<CaseEntity> builder)
    {
        builder.ToTable("cases", schema: "case");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.PublicId).IsUnique();

        builder.HasIndex(e => e.DisplayCode)
            .IsUnique()
            .HasFilter("\"DisplayCode\" IS NOT NULL");

        // Composite index per architecture diagram ("composite (status, case_type)").
        builder.HasIndex(e => new { e.Status, e.CaseType });

        builder.HasMany(e => e.TcnProfiles)
            .WithOne()
            .HasForeignKey(e => e.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
