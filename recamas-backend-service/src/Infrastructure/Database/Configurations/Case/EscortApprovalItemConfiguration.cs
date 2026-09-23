using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class EscortApprovalItemConfiguration : IEntityTypeConfiguration<EscortApprovalItem>
{
    public void Configure(EntityTypeBuilder<EscortApprovalItem> builder)
    {
        builder.ToTable("escort_approval_items", schema: "cases");
        builder.Property(e => e.EscortType).HasComment("AIUOfficer | MedicalProfessional | Other");
    }
}
