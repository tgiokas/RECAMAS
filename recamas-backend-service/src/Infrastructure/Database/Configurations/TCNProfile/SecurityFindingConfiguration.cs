using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class SecurityFindingConfiguration : IEntityTypeConfiguration<SecurityFinding>
{
    public void Configure(EntityTypeBuilder<SecurityFinding> builder)
    {
        builder.ToTable("security_findings", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.CaseTcnId).HasComment("FK → CaseTcn (αν εντοπίστηκε μέσα σε case)");
        builder.Property(e => e.FindingType).HasComment("CriminalRecord | RestrictiveActivity | Other");
        builder.Property(e => e.SeverityLevel).HasComment("Low | Medium | High");
        builder.Property(e => e.Details).HasComment("Ελεύθερο κείμενο περιγραφής — free text");
        builder.Property(e => e.AttachmentPath).HasComment("Storage path");
    }
}
