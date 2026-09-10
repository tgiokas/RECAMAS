using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class IdentityDocumentConfiguration : IEntityTypeConfiguration<IdentityDocument>
{
    public void Configure(EntityTypeBuilder<IdentityDocument> builder)
    {
        builder.ToTable("identity_documents", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Source).HasComment("ARS | CASS | RECAMAS | Case");
        builder.Property(e => e.DocumentType).HasComment("Passport | CountryIssuedId | Other");
        builder.Property(e => e.IsTravelDocument).HasComment("True → μπορεί να χρησιμοποιηθεί σε Return Case");
        builder.Property(e => e.DocumentNumber).HasComment("Αριθμός εγγράφου — free text (διαφορετική μορφή ανά χώρα)");
        builder.Property(e => e.IssuingCountryCode).HasComment("ISO 3166-1 alpha-2 — string (standard country code)");
        builder.Property(e => e.IssuingAuthority).HasComment("Αρχή έκδοσης — free text (ανοιχτό set ανά χώρα)");
        builder.Property(e => e.AttachmentPath).HasComment("Storage path του scan");
    }
}
