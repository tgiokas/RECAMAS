using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CounsellingSessionConfiguration : IEntityTypeConfiguration<CounsellingSession>
{
    public void Configure(EntityTypeBuilder<CounsellingSession> builder)
    {
        builder.ToTable("counselling_sessions", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.PreferredLanguage).HasComment("ISO 639-1 language code — string (standard, ανοιχτό set γλωσσών)");
        builder.Property(e => e.EnglishProficiency).HasComment("Fluent | Some | None");
        builder.Property(e => e.InterpreterDetails).HasComment("Όνομα / γλώσσα / τρόπος — free text");
        builder.Property(e => e.CurrentStatus).HasComment("AsylumSeeker | Student | Worker | Other");
        builder.Property(e => e.ReturnMotivation).HasComment("Long text — free text");
        builder.Property(e => e.EntryMethod).HasComment("RegularCrossing | ThroughOccupiedArea | Other");
        builder.Property(e => e.EntryMethodDetails).HasComment("Free text (αν Other)");
        builder.Property(e => e.HowHeardAboutProgram).HasComment("Office | FRONTEX | NGO | Friend | SocialMedia | Other");
        builder.Property(e => e.TravelDocumentLocation).HasComment("WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld");
        builder.Property(e => e.PoliceReportReference).HasComment("Αριθμός αναφοράς αστυνομίας — free text");
        builder.Property(e => e.FinesDebtsDetails).HasComment("Λεπτομέρειες — free text");
        builder.Property(e => e.AsylumAppealStatus).HasComment("NoAppeal | Open | Closed");
        builder.Property(e => e.OtherLegalSituation).HasComment("Free text (αν B1 = Other)");
        builder.Property(e => e.TravelGroup).HasComment("Alone | WithSpouse | WithChildren | WithSpouseAndChildren | SingleParentWithChild");
        builder.Property(e => e.OtherParentLocation).HasComment("Τοποθεσία άλλου γονέα — free text");
        builder.Property(e => e.FamilyInCyprusDetails).HasComment("Free text");
        builder.Property(e => e.FamilyReturnDetails).HasComment("Free text");
        builder.Property(e => e.VulnerabilityTypes).HasComment("[Flags] enum — None/Pregnancy/Medical/Disability/UnaccompaniedMinor/Elderly/SingleParent/TraffickingVictim/MentalHealth/Other");
        builder.Property(e => e.AccommodationDetails).HasComment("Free text");
        builder.Property(e => e.MedicalDetails).HasComment("Confidential — free text");
        builder.Property(e => e.AirportAssistanceDetails).HasComment("Free text");
        builder.Property(e => e.PreferredProgram).HasComment("AVRCyprus | EURP | Undecided");
        builder.Property(e => e.WishesToProceed).HasComment("Yes | No | StillConsidering");
        builder.Property(e => e.ContactPhone).HasComment("Τηλέφωνο — free text");
        builder.Property(e => e.CurrentAddress).HasComment("Διεύθυνση — free text");
        builder.Property(e => e.PreferredAirport).HasComment("IATA code ή free text (best effort)");
    }
}
