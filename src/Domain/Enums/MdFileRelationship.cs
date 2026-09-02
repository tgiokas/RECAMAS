namespace RECAMAS.Domain.Enums;

/// Per Implementation Specs Table 3: "Relationship to MD File | String | ARS |
/// Role of the person in the MD File: Principal / Main Dependant / Dependant."
public enum MdFileRelationship
{
    Principal = 1,
    MainDependant = 2,
    Dependant = 3,
}
