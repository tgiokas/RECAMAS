using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Rules;

namespace RECAMAS.Infrastructure.Persistence.Configurations.Rules;

public class RuleVersionConfiguration : IEntityTypeConfiguration<RuleVersion>
{
    public void Configure(EntityTypeBuilder<RuleVersion> builder)
    {
        builder.ToTable("rule_versions", schema: "rules");
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.RuleId, e.VersionNumber }).IsUnique();

        // Architecture decision: "Structured JSON tree, nested AND/OR groups
        // of field-operator-value" — stored as jsonb, not parsed at this layer.
        builder.Property(e => e.ConditionsJson).HasColumnType("jsonb");
        builder.Property(e => e.ThenActionsJson).HasColumnType("jsonb");
    }
}
