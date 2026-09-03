using RECAMAS.Domain.Common;

namespace RECAMAS.Domain.Entities.TCNProfile;

/// Specs Table 14 — self-referencing link between two TCN profiles (e.g.
/// family members found via the same ARS Folder Number, Specs 2.2.4).
/// RECAMAS ID/Name/ARC shown in the UI table are denormalized display-only
/// fields resolved by joining to LinkedProfileId — not stored here.
/// Relationship has no defined value set in the Specs ("Relationship | Enum",
/// no options given) — kept as a provisional string pending that.
/// One-directional by design: if the relationship is mutual, the Application
/// layer creates both rows rather than this entity implying symmetry.
public class TCNProfileLink : BaseEntity
{
    public long TCNProfileId { get; set; }
    public long LinkedProfileId { get; set; }

    public string? Relationship { get; set; }
}
