using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.ReturnImplementation;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ImplementationDocumentConfiguration : IEntityTypeConfiguration<ImplementationDocument>
{
    public void Configure(EntityTypeBuilder<ImplementationDocument> builder)
    {
        builder.ToTable("implementation_documents", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.AttachmentPath).HasComment("UUID filename");
        builder.Property(e => e.MimeType).HasComment("IANA MIME type — string (standard)");
    }
}
