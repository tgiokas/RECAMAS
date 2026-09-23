using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseAssignmentConfiguration : IEntityTypeConfiguration<CaseAssignment>
{
    public void Configure(EntityTypeBuilder<CaseAssignment> builder)
    {
        builder.ToTable("case_assignments", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.UnassignedReason).HasComment("Submitted | ManualUnassign");
    }
}
