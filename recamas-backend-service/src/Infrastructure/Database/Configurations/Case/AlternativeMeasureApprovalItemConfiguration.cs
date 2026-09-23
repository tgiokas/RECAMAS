using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AlternativeMeasureApprovalItemConfiguration : IEntityTypeConfiguration<AlternativeMeasureApprovalItem>
{
    public void Configure(EntityTypeBuilder<AlternativeMeasureApprovalItem> builder)
    {
        builder.ToTable("alternative_measure_approval_items", schema: "cases");
    }
}
