using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class TravelDocumentIssuanceConfiguration : IEntityTypeConfiguration<TravelDocumentIssuance>
{
    public void Configure(EntityTypeBuilder<TravelDocumentIssuance> builder)
    {
        builder.ToTable("travel_document_issuances", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.HasOne(e => e.Case)
            .WithMany()
            .HasForeignKey(e => e.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(e => e.IssuanceId).HasComment("System-generated — string");
        builder.Property(e => e.RequestingAuthority).HasComment("MigrationDepartment | AIU | Other");
        builder.Property(e => e.IssuingCountryCode).HasComment("ISO 3166-1 alpha-2 — string");
        builder.Property(e => e.IssuingAuthority).HasComment("Αρχή έκδοσης — free text (ανά χώρα διαφέρει)");
        builder.Property(e => e.Status).HasComment("Requested | Issued | Rejected");
        builder.Property(e => e.IssuedDocumentNumber).HasComment("Free text");
        builder.HasIndex(e => e.IssuanceId).IsUnique();
    }
}
