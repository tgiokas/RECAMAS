using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

/// Εγγραφή κράτησης — κάθε αλλαγή = νέα εγγραφή (§4.5.2.5.1)
public class DetentionRecord : BaseEntity
{
    public long ForcedReturnCaseId { get; set; }
    public long TcnProfileId { get; set; }
    public long DetentionCenterId { get; set; }
    public long? DetentionWingId { get; set; }
    public long? DetentionRoomId { get; set; }
    public string? Bed { get; set; }                     // [ΠΡΟΣΤΕΘΗΚΕ] — έλειπε, βλ. case_detention_period.bed
                                                           // PDF ref: §5.2.2 "Detention Centers" — "the above metrics can also be applied to more
                                                           // detailed levels such as 'Wing' and 'Beds'"

    public DateOnly DetentionStartDate { get; set; }
    public DateOnly? DetentionEndDate { get; set; }
    public int? CumulativeDetentionDays { get; set; }   // Calculated
    public int? MaximumDetentionPeriodDays { get; set; } // Από admin parameter
    public bool IsInitialEntry { get; set; }

    public ICollection<TemporaryCheckout> TemporaryCheckouts { get; set; } = [];

    public ForcedReturnCase ForcedReturnCase { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
    public DetentionCenter DetentionCenter { get; set; } = null!;
}
