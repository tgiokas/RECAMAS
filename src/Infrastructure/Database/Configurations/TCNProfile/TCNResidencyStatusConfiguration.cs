using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNResidencyStatusConfiguration : IEntityTypeConfiguration<TCNResidencyStatus>
{
    public void Configure(EntityTypeBuilder<TCNResidencyStatus> builder)
    {
        builder.ToTable("tcn_residency_statuses", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
