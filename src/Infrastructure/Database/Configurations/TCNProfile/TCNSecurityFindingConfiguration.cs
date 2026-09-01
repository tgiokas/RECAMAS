using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNSecurityFindingConfiguration : IEntityTypeConfiguration<TCNSecurityFinding>
{
    public void Configure(EntityTypeBuilder<TCNSecurityFinding> builder)
    {
        builder.ToTable("tcn_security_findings", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
