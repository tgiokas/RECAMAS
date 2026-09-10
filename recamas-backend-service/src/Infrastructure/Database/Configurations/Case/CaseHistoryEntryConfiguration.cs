using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseHistoryEntryConfiguration : IEntityTypeConfiguration<CaseHistoryEntry>
{
    public void Configure(EntityTypeBuilder<CaseHistoryEntry> builder)
    {
        builder.ToTable("case_history_entries", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.EntryType).HasComment("EntryCreation | DataUpdate | Assignment | Action | StageStatusChange | FlagChange");
        builder.Property(e => e.FieldName).HasComment("Όνομα πεδίου — string (reflection-based)");
        builder.Property(e => e.PreviousValue).HasComment("JSON serialized — string");
        builder.Property(e => e.UpdatedValue).HasComment("JSON serialized — string");
        builder.Property(e => e.PreviousAssignment).HasComment("\"UserName (RoleName)\" — string");
        builder.Property(e => e.ActionDescription).HasComment("Περιγραφή — free text");
        builder.Property(e => e.EntryTypeName).HasComment("Τύπος εγγραφής — free text (πχ. \"TravelDocument\")");
    }
}
