using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Domain.Entities.Case;

/// LIGHT SKELETON — enough shape for the team to build against in parallel,
/// not a finished data model. Locks in the one decision that's already
/// settled (architecture decision log): one Case table with shared columns,
/// plus a 1:1 detail table per CaseType (<see cref="AvrCaseDetail"/>,
/// <see cref="ForcedReturnCaseDetail"/>, <see cref="VoluntaryReturnOwnMeansCaseDetail"/>).
///
/// NOT modeled here yet — real design work, each a separate pass:
///  - Per-CaseType Stage/Status state machines (Specs 4.4.1/4.5.1/4.6.1 give
///    the full stage/status tables per type; Stage/Status below are placeholder
///    strings, not the real fixed enums the architecture decision calls for).
///  - The AVR Counselling questionnaire (Specs 4.4.2.2, ~40 conditional fields).
///  - Approval Items, Requests, Case History, flags (Specs 4.3.4/4.3.7/4.3.8).
public class Case : BaseEntity
{
    /// Human-facing case code, e.g. "AVR-2026-0117" per the Specs's own mockup captions.
    public string? DisplayCode { get; set; }

    public CaseType CaseType { get; set; }

    /// Master-data code (e.g. "AVR Cyprus", "EURP") — see TCNProfile remarks on "Enum" fields that are really admin-configurable lists.
    public string? Program { get; set; }

    /// TODO: replace with the real per-CaseType Stage enum once each workflow is designed.
    public string? Stage { get; set; }

    /// TODO: replace with the real per-CaseType Status enum once each workflow is designed.
    public string? Status { get; set; }

    public DateTimeOffset? InitiationDateTime { get; set; }

    /// Master-data code: MD or a specific A&amp;IU office.
    public string? InitiationOffice { get; set; }

    public string? ImplementationOffice { get; set; }

    public string? ReturnCountry { get; set; }

    public string? ReturnReason { get; set; }

    public List<CaseTcnProfile> TcnProfiles { get; set; } = [];
}
