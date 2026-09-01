using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNAppealConfiguration : IEntityTypeConfiguration<TCNAppeal>
{
    public void Configure(EntityTypeBuilder<TCNAppeal> builder)
    {
        builder.ToTable("tcn_appeals", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
