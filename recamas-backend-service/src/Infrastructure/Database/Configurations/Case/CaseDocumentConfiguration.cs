using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseDocumentConfiguration : IEntityTypeConfiguration<CaseDocument>
{
    public void Configure(EntityTypeBuilder<CaseDocument> builder)
    {
        builder.ToTable("case_documents", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TcnProfileId).HasComment("Null αν αφορά το case συνολικά");
        builder.Property(e => e.Kind).HasComment("Generated | Uploaded");
        builder.Property(e => e.Description).HasComment("Free text");
        builder.Property(e => e.AttachmentPath).HasComment("Storage path — UUID filename (§12.5.16)");
        builder.Property(e => e.OriginalFilename).HasComment("Αρχικό όνομα πριν rename");
        builder.Property(e => e.MimeType).HasComment("MIME type για validation — string (standard IANA)");
        builder.Property(e => e.JccTransactionId).HasComment("JCC reference — free text (external system format)");
    }
}
