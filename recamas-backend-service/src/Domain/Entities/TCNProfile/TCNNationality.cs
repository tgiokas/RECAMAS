using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// One declared nationality for a TCN. Table 3 notes a profile can have
/// "Multiple declared, with identification status" — the exact identification-status
/// value set isn't given in the Specs, so <see cref="IdentificationStatus"/> is a
/// free-form code for now (provisional, pending the Master Data list for it).
public class TCNNationality : BaseEntity
{
    public long TCNProfileId { get; set; }

    /// Master-data country code (see TCNProfile class remarks on "Enum" fields).
    public required string NationalityCode { get; set; }

    public bool IsPrimary { get; set; }

    public string? IdentificationStatus { get; set; }
}
