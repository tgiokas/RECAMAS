using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ByOwnReturnDecisionIssuanceConfiguration : IEntityTypeConfiguration<ByOwnReturnDecisionIssuance>
{
    public void Configure(EntityTypeBuilder<ByOwnReturnDecisionIssuance> builder)
    {
        builder.ToTable("by_own_return_decision_issuances", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.DecisionId).HasComment("System-generated — string");
        builder.Property(e => e.DocumentName).HasComment("Free text");
    }
}
