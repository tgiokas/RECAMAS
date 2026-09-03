using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Reports;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.ToTable("document_templates", schema: "reports");
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.CaseType, e.Title }).IsUnique();
    }
}
