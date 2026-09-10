using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseTravelDocumentConfiguration : IEntityTypeConfiguration<CaseTravelDocument>
{
    public void Configure(EntityTypeBuilder<CaseTravelDocument> builder)
    {
        builder.ToTable("case_travel_documents", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.DocumentNumber).HasComment("Αριθμός εγγράφου — free text");
        builder.Property(e => e.IssuingCountryCode).HasComment("ISO 3166-1 alpha-2 — string (standard code)");
        builder.Property(e => e.IssuingAuthority).HasComment("Αρχή έκδοσης — free text (ανοιχτό set)");
        builder.Property(e => e.IssuedForReturnCase).HasComment("True αν από Travel Doc Issuance process");
        builder.Property(e => e.PhysicalLocation).HasComment("WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld | AIUOffice");
        builder.Property(e => e.SourceIdentityDocumentId).HasComment("FK → IdentityDocument (αν από profile)");
    }
}
