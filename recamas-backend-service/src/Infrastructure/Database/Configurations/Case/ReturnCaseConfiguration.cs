using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ReturnCaseConfiguration : IEntityTypeConfiguration<ReturnCase>
{
    public void Configure(EntityTypeBuilder<ReturnCase> builder)
    {
        builder.ToTable("return_cases", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.CaseId).HasComment("Human-readable system-generated ID (πχ. AVR-2026-001)");
        builder.Property(e => e.CaseType).HasComment("AVR | ForcedReturn | ByOwn");
        builder.Property(e => e.Stage).HasComment("Counselling | ApplicationProcessing | Detention | κλπ");
        builder.Property(e => e.Status).HasComment("Initiated | OnHold | PendingApproval | κλπ");
        builder.Property(e => e.InitiationDateTime).HasComment("System-generated κατά δημιουργία");
        builder.Property(e => e.InitiationOffice).HasComment("MD | συγκεκριμένο A&IU office");
        builder.Property(e => e.ImplementationOffice).HasComment("A&IU office υλοποίησης — free text (codelist-driven από admin)");
        builder.Property(e => e.ReturnCountryCode).HasComment("ISO 3166-1 alpha-2 — string (standard country code)");
        builder.Property(e => e.ReturnReason).HasComment("ILMigrant | AsylumSeeker | AsylumRejection | κλπ");
        builder.Property(e => e.InternationalFramework).HasComment("EUReadmission | Bilateral | κλπ");
        builder.Property(e => e.FlagNeedsAttention).HasComment("Interface update σε TCN profile (auto On Hold)");
        builder.Property(e => e.FlagException).HasComment("Χώρα εκτός προγράμματος");
        builder.Property(e => e.FlagProgramSwitch).HasComment("AVR Cyprus ↔ EURP switch");
        builder.Property(e => e.OnHoldReason).HasComment("ManuallyPlaced | InterfaceUpdate");
        builder.Property(e => e.CancellationReason).HasComment("Ελεύθερο κείμενο — απαιτείται (§4.3.2)");
        builder.HasIndex(e => e.CaseId).IsUnique();
    }
}
