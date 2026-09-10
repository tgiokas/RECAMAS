using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Εγγραφή TCN στη Stoplist — από Police Database (§3.3.3.1)
public class StoplistEntry : BaseEntity
{
    public long TcnProfileId { get; set; }

    public bool StoplistHit { get; set; }               // Βρέθηκε ή όχι στη stoplist
    public string? StoplistReason { get; set; }         // Λόγος εγγραφής — free text (από Police DB)
    public string? UniqueEntryBanNumber { get; set; }   // Μοναδικός αριθμός απαγόρευσης — free text (Police DB format)
    public DateOnly? StoplistEntryDate { get; set; }
    public int? EntryBanDurationMonths { get; set; }    // Από Case
    public DateOnly? EntryBanExpirationDate { get; set; } // Υπολογίζεται από Implementation
    public DateTimeOffset? LastSyncedAt { get; set; }   // Τελευταίος συγχρονισμός από Police DB
    public DataSourceType Source { get; set; }           // [ΠΡΟΣΤΕΘΗΚΕ] default POLICE_DB — έλειπε, βλ. tcn_stoplist_entry.source
                                                           // PDF ref: §9.5 "Stoplist" — Stoplist is a system maintained in the Police Database (§9.5, §9.5.1 Interface execution)

    public TcnProfile TcnProfile { get; set; } = null!;
}
