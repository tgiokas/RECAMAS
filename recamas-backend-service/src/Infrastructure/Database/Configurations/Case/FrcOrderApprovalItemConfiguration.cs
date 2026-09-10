using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class FrcOrderApprovalItemConfiguration : IEntityTypeConfiguration<FrcOrderApprovalItem>
{
    public void Configure(EntityTypeBuilder<FrcOrderApprovalItem> builder)
    {
        builder.ToTable("frc_order_approval_items", schema: "cases");
        builder.Property(e => e.OrderType).HasComment("DetentionOrder | DeportationOrder");
        builder.Property(e => e.LegalBasis).HasComment("Νομική βάση — string: codelist τιμή ή free text (§4.5.2.4.1: \"list of values OR free text\")");
    }
}
