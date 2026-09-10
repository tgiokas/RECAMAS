using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ReturnDecisionConfiguration : IEntityTypeConfiguration<ReturnDecision>
{
    public void Configure(EntityTypeBuilder<ReturnDecision> builder)
    {
        builder.ToTable("return_decisions", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.IssuingAuthority).HasComment("ARS | CASS | MD | CAS | Other");
        builder.Property(e => e.DecisionText).HasComment("Κείμενο απόφασης — free text");
        builder.Property(e => e.DecisionFilePath).HasComment("Storage path");
        builder.Property(e => e.IssuingCaseId).HasComment("FK → ReturnCase (αν εκδόθηκε μέσω Case)");
    }
}
