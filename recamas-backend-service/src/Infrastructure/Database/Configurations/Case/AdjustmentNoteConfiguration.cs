using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AdjustmentNoteConfiguration : IEntityTypeConfiguration<AdjustmentNote>
{
    public void Configure(EntityTypeBuilder<AdjustmentNote> builder)
    {
        builder.ToTable("adjustment_notes", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Notes).HasComment("Λόγος — free text");
    }
}
