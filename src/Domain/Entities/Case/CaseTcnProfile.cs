using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.Case;

/// Join entity — a case can involve multiple TCN profiles (Specs 4.3.1: "Select
/// the TCN profiles involved in the case"), e.g. a family travelling together.
/// Crosses the case/tcn_profile schema boundary with a real FK schema-per-module
/// still shares one Postgres database, so referential integrity across modules
/// is available and worth keeping even though the modules are logically separate.
public class CaseTcnProfile : BaseEntity
{
    public long CaseId { get; set; }

    public long TCNProfileId { get; set; }
}
