namespace RECAMAS.Domain.Enums;

/// Fixed, stable set — decided during architecture design to be a code enum,
/// NOT a lookup table, since these 3 values are procedurally/legally fixed
/// and unlikely to ever gain a 4th member without a much larger scope change.
public enum CaseType
{
    AssistedVoluntaryReturn = 1,
    ForcedReturn = 2,
    VoluntaryReturnOwnMeans = 3,
}
