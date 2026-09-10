using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class BusinessRuleConfiguration : IEntityTypeConfiguration<BusinessRule>
{
    public void Configure(EntityTypeBuilder<BusinessRule> builder)
    {
        builder.ToTable("business_rules", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.RuleId).HasComment("Human-readable system ID — string");
        builder.Property(e => e.Programme).HasComment("Programme code ή null (= AllProgrammes) — string: codelist value");
        builder.Property(e => e.ConditionExpression).HasComment("JSON DSL — string (structured expression tree)").HasColumnType("jsonb");
        builder.Property(e => e.TargetFieldOrDocumentType).HasComment("Field name ή doc type — string (dynamic)");
        builder.Property(e => e.UserFacingMessage).HasComment("Μήνυμα — free text");
        builder.Property(e => e.RuleStatus).HasComment("Draft | Scheduled | Effective | Expired");
        builder.HasIndex(e => e.RuleId).IsUnique();
    }
}
