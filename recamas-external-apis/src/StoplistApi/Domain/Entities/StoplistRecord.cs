using Shared.Domain.Common;
using Shared.Domain.Enums;

namespace StoplistApi.Domain.Entities;

/// <summary>
/// Stoplist record — Police Database, entry-ban register.
/// Mirrors §9.5 Implementation Study (Tables 166-168).
/// </summary>
public class StoplistRecord : BaseEntity
{
    // === REQUEST fields (used for search) ===
    public string Arc { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Nationality Nationality { get; set; }
    public string? PassportNo { get; set; }
    public DateTime DateOfBirth { get; set; }

    // === RESPONSE fields (Table 168) ===
    public bool IsOnStoplist { get; set; }
    public string? UniqueEntryBanNumber { get; set; }
    public DateTime? StoplistEntryDate { get; set; }
}
