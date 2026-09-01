using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.Case;

public class CaseTcnProfileConfiguration : IEntityTypeConfiguration<CaseTcnProfile>
{
    public void Configure(EntityTypeBuilder<CaseTcnProfile> builder)
    {
        builder.ToTable("case_tcn_profiles", schema: "case");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.CaseId, e.TCNProfileId }).IsUnique();

        // Cross-schema FK — see CaseTcnProfile remarks: no navigation on the
        // TCNProfile side, just referential integrity.
        builder.HasOne<TCNProfileEntity>()
            .WithMany()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
