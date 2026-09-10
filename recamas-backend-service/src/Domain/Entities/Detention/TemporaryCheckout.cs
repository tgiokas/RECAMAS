using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Detention;

/// Προσωρινή αποφυλάκιση (§4.5.2.5.2)
public class TemporaryCheckout : BaseEntity
{
    public long DetentionRecordId { get; set; }

    public DateOnly CheckoutDate { get; set; }
    public DateOnly? ExpectedCheckinDate { get; set; }
    public TemporaryCheckoutReason CheckoutReason { get; set; } // Medical | DoctorVisit | Embassy | Other
    public DateOnly? ActualCheckinDate { get; set; }
    public string? Notes { get; set; }                  // Free text

    public DetentionRecord DetentionRecord { get; set; } = null!;
}
