using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;
using CaseEntity = RECAMAS.Domain.Entities.Case.Case;

namespace RECAMAS.Infrastructure.Database.Configurations.Detention;

public class DetentionOrderConfiguration : IEntityTypeConfiguration<DetentionOrder>
{
    public void Configure(EntityTypeBuilder<DetentionOrder> builder)
    {
        builder.ToTable("detention_orders", schema: "detention");
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.CaseId);

        // Cross-schema FK to case.cases — no navigation on the Case side.
        builder.HasOne<CaseEntity>()
            .WithMany()
            .HasForeignKey(e => e.CaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DetentionFacility>()
            .WithMany()
            .HasForeignKey(e => e.DetentionFacilityId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
