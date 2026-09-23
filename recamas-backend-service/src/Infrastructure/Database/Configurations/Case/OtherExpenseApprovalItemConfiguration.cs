using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class OtherExpenseApprovalItemConfiguration : IEntityTypeConfiguration<OtherExpenseApprovalItem>
{
    public void Configure(EntityTypeBuilder<OtherExpenseApprovalItem> builder)
    {
        builder.ToTable("other_expense_approval_items", schema: "cases");
        builder.Property(e => e.ExpenseType).HasComment("Accommodation | Translation | Medical | Administrative | Other");
        builder.Property(e => e.Amount).HasPrecision(18, 2);
    }
}
