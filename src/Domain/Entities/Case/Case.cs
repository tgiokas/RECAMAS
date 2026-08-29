using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// <summary>
/// LIGHT SKELETON — enough shape for the team to build against in parallel,
/// not a finished data model. Locks in the one decision that's already
/// settled (architecture decision log): one Case table with shared columns,
/// plus a 1:1 detail table per CaseType (<see cref="AvrCaseDetail"/>,
/// <see cref="ForcedReturnCaseDetail"/>, <see cref="VoluntaryReturnOwnMeansCaseDetail"/>).
///
/// NOT modeled here yet — real design work, each a separate pass:
///  - Per-CaseType Stage/Status state machines (Study 4.4.1/4.5.1/4.6.1 give
///    the full stage/status tables per type; Stage/Status below are placeholder
///    strings, not the real fixed enums the architecture decision calls for).
///  - The AVR Counselling questionnaire (Study 4.4.2.2, ~40 conditional fields).
///  - Approval Items, Requests, Case History, flags (Study 4.3.4/4.3.7/4.3.8).
/// </summary>
public class Case : BaseEntity
{
    /// <summary>Human-facing case code, e.g. "AVR-2026-0117" per the Study's own mockup captions.</summary>
    public string? DisplayCode { get; set; }

    public CaseType CaseType { get; set; }

    /// <summary>Master-data code (e.g. "AVR Cyprus", "EURP") — see TCNProfile remarks on "Enum" fields that are really admin-configurable lists.</summary>
    public string? Program { get; set; }

    /// <summary>TODO: replace with the real per-CaseType Stage enum once each workflow is designed.</summary>
    public string? Stage { get; set; }

    /// <summary>TODO: replace with the real per-CaseType Status enum once each workflow is designed.</summary>
    public string? Status { get; set; }

    public DateTimeOffset? InitiationDateTime { get; set; }

    /// <summary>Master-data code: MD or a specific A&amp;IU office.</summary>
    public string? InitiationOffice { get; set; }

    public string? ImplementationOffice { get; set; }

    public string? ReturnCountry { get; set; }

    public string? ReturnReason { get; set; }

    public List<CaseTcnProfile> TcnProfiles { get; set; } = [];
}
