using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// <summary>
/// One declared nationality for a TCN. Table 3 notes a profile can have
/// "Multiple declared, with identification status" — the exact identification-status
/// value set isn't given in the Study, so <see cref="IdentificationStatus"/> is a
/// free-form code for now (provisional, pending the Master Data list for it).
/// </summary>
public class TCNNationality : BaseEntity
{
    public long TCNProfileId { get; set; }

    /// <summary>Master-data country code (see TCNProfile class remarks on "Enum" fields).</summary>
    public required string NationalityCode { get; set; }

    public bool IsPrimary { get; set; }

    public string? IdentificationStatus { get; set; }
}
