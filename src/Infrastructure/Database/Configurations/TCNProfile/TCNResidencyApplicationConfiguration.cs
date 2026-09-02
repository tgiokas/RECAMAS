using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class TCNResidencyApplicationConfiguration : IEntityTypeConfiguration<TCNResidencyApplication>
{
    public void Configure(EntityTypeBuilder<TCNResidencyApplication> builder)
    {
        builder.ToTable("tcn_residency_applications", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
