using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNInternationalProtectionStatusConfiguration : IEntityTypeConfiguration<TCNInternationalProtectionStatus>
{
    public void Configure(EntityTypeBuilder<TCNInternationalProtectionStatus> builder)
    {
        builder.ToTable("tcn_international_protection_statuses", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
