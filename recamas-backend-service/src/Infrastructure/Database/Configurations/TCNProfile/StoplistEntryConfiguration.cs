using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class StoplistEntryConfiguration : IEntityTypeConfiguration<StoplistEntry>
{
    public void Configure(EntityTypeBuilder<StoplistEntry> builder)
    {
        builder.ToTable("stoplist_entries", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.StoplistHit).HasComment("Βρέθηκε ή όχι στη stoplist");
        builder.Property(e => e.StoplistReason).HasComment("Λόγος εγγραφής — free text (από Police DB)");
        builder.Property(e => e.UniqueEntryBanNumber).HasComment("Μοναδικός αριθμός απαγόρευσης — free text (Police DB format)");
        builder.Property(e => e.EntryBanDurationMonths).HasComment("Από Case");
        builder.Property(e => e.EntryBanExpirationDate).HasComment("Υπολογίζεται από Implementation");
        builder.Property(e => e.LastSyncedAt).HasComment("Τελευταίος συγχρονισμός από Police DB");
        builder.Property(e => e.Source).HasComment("[ΠΡΟΣΤΕΘΗΚΕ] default POLICE_DB — έλειπε, βλ. tcn_stoplist_entry.source PDF ref: §9.5 \"Stoplist\" — Stoplist is a system maintained in the Police Database (§9.5, §9.5.1 Interface execution)");
    }
}
