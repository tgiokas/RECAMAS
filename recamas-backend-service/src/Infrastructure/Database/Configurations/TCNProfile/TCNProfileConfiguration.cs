using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;
using Pgvector;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class TcnProfileConfiguration : IEntityTypeConfiguration<TcnProfile>
{
    public void Configure(EntityTypeBuilder<TcnProfile> builder)
    {
        builder.ToTable("tcn_profiles", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.RecamasId).HasComment("Μοναδικό system-generated ID (πχ. TCN-2026-00001)");
        builder.Property(e => e.Arc).HasComment("Alien Registration Card number — από ARS interface");
        builder.Property(e => e.EurodacNumber).HasComment("EURODAC biometric reference — από CASS");
        builder.Property(e => e.FirstNameEl).HasComment("Όνομα στα Ελληνικά — από ARS/Manual");
        builder.Property(e => e.FirstNameEn).HasComment("Όνομα στα Αγγλικά — από ARS/Manual");
        builder.Property(e => e.MiddleNameEl).HasComment("Μεσαίο όνομα EL");
        builder.Property(e => e.MiddleNameEn).HasComment("Μεσαίο όνομα EN");
        builder.Property(e => e.LastNameEl).HasComment("Επώνυμο EL");
        builder.Property(e => e.LastNameEn).HasComment("Επώνυμο EN");
        builder.Property(e => e.Gender).HasComment("M / F / U (Unknown) — από ARS");
        builder.Property(e => e.DateOfBirth).HasComment("Ημερομηνία γέννησης — από ARS/Manual");
        builder.Property(e => e.PlaceOfBirth).HasComment("Τόπος γέννησης — free text (πόλη ή περιοχή, όχι enum: αχανές set)");
        builder.Property(e => e.MdFileNo).HasComment("Migration Department file number — από ARS");
        builder.Property(e => e.ArsFolderId).HasComment("[ΠΡΟΣΤΕΘΗΚΕ] Βάση για πρόταση linked-profile — έλειπε, βλ. tcn_profile.ars_folder_id PDF ref: §2.2.4 \"Linked Profile Suggestion\" — \"Based on the ARS Folder Number, RECAMAS performs an interface call to ARS and retrieves any TCN Profiles that have the same ARS Folder Number\"");
        builder.Property(e => e.MdFileRelationship).HasComment("Ρόλος στο MD file: Principal / MainDependant / Dependant");
        builder.Property(e => e.CassFileNo).HasComment("Cyprus Asylum Service file number — από CASS");
        builder.Property(e => e.CassAddress).HasComment("Διεύθυνση από CASS (read-only, free text)");
        builder.Property(e => e.MdAddress).HasComment("Διεύθυνση από ARS (read-only, free text)");
        builder.Property(e => e.CassPhone).HasComment("Τηλέφωνο από CASS (read-only)");
        builder.Property(e => e.MdPhone).HasComment("Τηλέφωνο από ARS (read-only)");
        builder.Property(e => e.PhotographStoragePath).HasComment("Path στο storage (ISO/IEC 19794-5:2011)");
        builder.Property(e => e.FingerprintNistPath).HasComment("Path NIST ITL 1-2011 file");
        builder.Property(e => e.FingerprintVector)
            .HasConversion(
                value => value == null ? null : new Vector(value),
                value => value == null ? null : value.ToArray())
            .HasColumnType("vector")
            .HasComment("pgvector embedding για biometric search");
        builder.Property(e => e.Status).HasComment("§3.4: AVRApplicationPending, AVRReturnPending, Departed, κλπ");
        builder.Property(e => e.FlagSecurityIssues).HasComment("§3.5: Υπάρχει τουλάχιστον ένα security issue item");
        builder.Property(e => e.FlagMinor).HasComment("§3.5: Ηλικία < 18");
        builder.Property(e => e.FlagNoArc).HasComment("§3.5: Δεν υπάρχει ARC");
        builder.Property(e => e.FlagNoTravelDocument).HasComment("§3.5: Δεν υπάρχει travel document για επιστροφή");
        builder.Property(e => e.IsAnonymized).HasComment("Έχει γίνει anonymization βάσει retention policy");
        builder.Property(e => e.AnonymizedAt).HasComment("Πότε έγινε anonymization");
        builder.Property(e => e.AnonymizedByUserId).HasComment("Ποιος έκανε anonymization");
        builder.Property(e => e.PrimarySource).HasComment("ARS | CASS | Manual — από που δημιουργήθηκε το profile");
        builder.HasIndex(e => e.RecamasId).IsUnique();
        builder.HasIndex(e => e.Arc);
    }
}
