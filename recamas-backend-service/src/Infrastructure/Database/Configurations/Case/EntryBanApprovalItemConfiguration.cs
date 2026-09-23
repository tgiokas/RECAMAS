using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class EntryBanApprovalItemConfiguration : IEntityTypeConfiguration<EntryBanApprovalItem>
{
    public void Configure(EntityTypeBuilder<EntryBanApprovalItem> builder)
    {
        builder.ToTable("entry_ban_approval_items", schema: "cases");
    }
}
