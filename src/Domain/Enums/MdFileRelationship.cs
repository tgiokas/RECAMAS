namespace RECAMAS.Domain.Enums;

/// <summary>
/// Per Implementation Study Table 3: "Relationship to MD File | String | ARS |
/// Role of the person in the MD File: Principal / Main Dependant / Dependant."
/// Modeled as an enum here even though the doc types it as String, since the
/// value set itself is explicitly fixed to these 3 roles.
/// </summary>
public enum MdFileRelationship
{
    Principal = 1,
    MainDependant = 2,
    Dependant = 3,
}
