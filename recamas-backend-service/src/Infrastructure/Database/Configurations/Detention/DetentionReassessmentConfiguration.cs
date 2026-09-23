using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionReassessmentConfiguration : IEntityTypeConfiguration<DetentionReassessment>
{
    public void Configure(EntityTypeBuilder<DetentionReassessment> builder)
    {
        builder.ToTable("detention_reassessments", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ReassessmentType).HasComment("Planned | AdHoc");
        builder.Property(e => e.Status).HasComment("Scheduled | Completed | Overdue");
        builder.Property(e => e.RelatedIssueId).HasComment("FK → DetentionUpdateIssue");
        builder.Property(e => e.AssessmentReportPath).HasComment("Storage path");
    }
}
