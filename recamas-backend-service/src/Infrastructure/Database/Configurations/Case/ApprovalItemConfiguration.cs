using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ApprovalItemConfiguration : IEntityTypeConfiguration<ApprovalItem>
{
    public void Configure(EntityTypeBuilder<ApprovalItem> builder)
    {
        builder.ToTable("approval_items", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.AssessmentDecision).HasComment("Pending | Approved | Rejected");
        builder.Property(e => e.ApproverNotes).HasComment("Σημειώσεις εγκριτή — free text");
        builder.Property(e => e.RevocationReason).HasComment("Λόγος ανάκλησης — free text");
        builder.Property(e => e.RelatedIssueId).HasComment("FK → DetentionUpdateIssue");
    }
}
