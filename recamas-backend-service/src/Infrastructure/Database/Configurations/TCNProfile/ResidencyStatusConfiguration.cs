using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ResidencyStatusConfiguration : IEntityTypeConfiguration<ResidencyStatus>
{
    public void Configure(EntityTypeBuilder<ResidencyStatus> builder)
    {
        builder.ToTable("residency_statuses", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.PermitType).HasComment("Τύπος άδειας παραμονής / ταξιδίου");
        builder.Property(e => e.ResidenceCategory).HasComment("Κατηγορία παραμονής");
        builder.Property(e => e.PurposeRnd).HasComment("Κωδικός / περιγραφή σκοπού παραμονής (RND) — free text, ARS-specific");
        builder.Property(e => e.Status).HasComment("Active | Expired | Revoked");
        builder.Property(e => e.ResidencyDocumentNumber).HasComment("Μοναδικός αριθμός άδειας — free text");
    }
}
