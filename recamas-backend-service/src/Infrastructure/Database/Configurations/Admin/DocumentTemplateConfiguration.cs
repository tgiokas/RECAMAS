using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.ToTable("document_templates", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.HasMany(e => e.PreviousVersions)
            .WithOne()
            .HasForeignKey(e => e.PreviousVersionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(e => e.Language).HasComment("Greek | English | Other");
        builder.Property(e => e.TemplateFilePath).HasComment("Storage path");
        builder.Property(e => e.PlaceholdersDefinition).HasComment("JSON — string (structured data)").HasColumnType("jsonb");
    }
}
