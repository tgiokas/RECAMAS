using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseReturnDecisionConfiguration : IEntityTypeConfiguration<CaseReturnDecision>
{
    public void Configure(EntityTypeBuilder<CaseReturnDecision> builder)
    {
        builder.ToTable("case_return_decisions", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.DecisionId).HasComment("System-generated ID — string (human-readable reference)");
        builder.Property(e => e.Source).HasComment("RECAMAS | ARS | CASS | Other");
        builder.Property(e => e.IssuingAuthority).HasComment("MD | CAS | ARS | Other");
        builder.Property(e => e.DecisionText).HasComment("Κείμενο απόφασης — free text");
    }
}
