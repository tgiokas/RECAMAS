using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ReturnDecisionApprovalItemConfiguration : IEntityTypeConfiguration<ReturnDecisionApprovalItem>
{
    public void Configure(EntityTypeBuilder<ReturnDecisionApprovalItem> builder)
    {
        builder.ToTable("return_decision_approval_items", schema: "cases");
        builder.Property(e => e.DecisionId).HasComment("System-generated — string");
        builder.Property(e => e.DocumentName).HasComment("Τίτλος εγγράφου — free text");
        builder.Property(e => e.DocumentLanguage).HasComment("Greek | English | Other");
        builder.Property(e => e.PreparedByUserId).HasComment("[ΠΡΟΣΤΕΘΗΚΕ] Ξεχωριστό από ApprovalItem.AssessedByUserId (=approved by) — έλειπε η διάκριση \"συνέταξε\" vs \"ενέκρινε\", βλ. case_return_decision.prepared_by PDF ref: ΟΧΙ άμεσο match — το Table 40 \"AVR Return Decisions\" (§4.4.2.4.1) έχει μόνο \"Approved by\". Αναλογία με Table 91 \"BOR Return Decision Issuance\" (§4.6.2.3), που έχει ρητό πεδίο \"Prepared by\" στην κλάση ByOwnReturnDecisionIssuance — χρειάζεται επιβεβαίωση αν ισχύει και εδώ");
    }
}
