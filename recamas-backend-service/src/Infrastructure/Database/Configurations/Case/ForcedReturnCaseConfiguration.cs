using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ForcedReturnCaseConfiguration : IEntityTypeConfiguration<ForcedReturnCase>
{
    public void Configure(EntityTypeBuilder<ForcedReturnCase> builder)
    {
        builder.ToTable("forced_return_cases", schema: "cases");
        builder.Ignore(e => e.PreReturnChecklist);
        builder.Ignore(e => e.TravelDocumentIssuances);
        builder.Property(e => e.Program).HasComment("EURP | Cyprus");
        builder.Property(e => e.ApprehensionOfficer).HasComment("Ονοματεπώνυμο — free text");
        builder.Property(e => e.ApprehensionLocation).HasComment("Τοποθεσία — free text (ανοιχτό)");
        builder.Property(e => e.ApprehensionJustification).HasComment("Αιτιολόγηση — free text");
        builder.Property(e => e.PreliminaryDetentionLocation).HasComment("Τοποθεσία — free text");
        builder.Property(e => e.EscapeRisk).HasComment("Low | Medium | High");
        builder.Property(e => e.AiuOfficerNotes).HasComment("Σημειώσεις — free text");
        builder.Property(e => e.SuggestionMemoPath).HasComment("Storage path");
    }
}
