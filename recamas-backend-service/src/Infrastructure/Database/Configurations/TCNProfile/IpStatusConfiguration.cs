using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class IpStatusConfiguration : IEntityTypeConfiguration<IpStatus>
{
    public void Configure(EntityTypeBuilder<IpStatus> builder)
    {
        builder.ToTable("ip_statuses", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TypeOfStatus).HasComment("RefugeeStatus | SubsidiaryProtection | Other");
        builder.Property(e => e.StatusDecision).HasComment("Pending | Approved | Rejected | Revoked | Other");
    }
}
