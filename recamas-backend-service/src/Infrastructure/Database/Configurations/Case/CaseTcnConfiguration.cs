using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseTcnConfiguration : IEntityTypeConfiguration<CaseTcn>
{
    public void Configure(EntityTypeBuilder<CaseTcn> builder)
    {
        builder.ToTable("case_tcns", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.FitToFlyAttachmentPath).HasComment("Storage path");
        builder.Property(e => e.PreReturnNoOpenIssues).HasComment("Μόνο FRC");
    }
}
