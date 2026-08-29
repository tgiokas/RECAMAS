using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>Study Table 7 (from CASS). Same history-not-overwrite treatment as TCNResidencyStatus — see its remarks.</summary>
public class TCNInternationalProtectionStatus : BaseEntity
{
    public long TCNProfileId { get; set; }

    public string? TypeOfStatus { get; set; }
    public DateOnly? DateOfGranting { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public DateOnly? DecisionDate { get; set; }
    public string? StatusDecision { get; set; }
}
