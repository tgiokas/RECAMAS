using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class TCNInternationalProtectionApplicationConfiguration : IEntityTypeConfiguration<TCNInternationalProtectionApplication>
{
    public void Configure(EntityTypeBuilder<TCNInternationalProtectionApplication> builder)
    {
        builder.ToTable("tcn_international_protection_applications", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
