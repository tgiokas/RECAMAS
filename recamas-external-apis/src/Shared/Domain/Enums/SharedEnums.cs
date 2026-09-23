namespace Shared.Domain.Enums;

public enum Nationality
{
    Syrian = 1, Afghan, Iraqi, Pakistani, Nigerian,
    Somali, Eritrean, Bangladeshi, Ethiopian, Egyptian,
    Albanian, Georgian, Moroccan, Algerian, Tunisian,
    Iranian, Sudanese, Congolese, Cameroonian, Ghanaian
}

public enum Gender { Male = 1, Female, Other }

public enum Airport
{
    LarnacaInternational = 1,
    PaphosInternational
}

public enum MovementType { Arrival = 1, Departure }

public enum PermitType
{
    TemporaryResidence = 1,
    PermanentResidence,
    AsylumSeeker,
    SubsidiaryProtection,
    HumanitarianProtection
}

public enum ResidenceCategory
{
    Family = 1, Employment, Education, Other
}

public enum ResidencyStatus
{
    Active = 1, Expired, Revoked, Pending
}

public enum IpStatusType
{
    RefugeeStatus = 1,
    SubsidiaryProtection,
    HumanitarianProtection,
    Rejected
}

public enum IpStatusDecision
{
    Granted = 1, Rejected, Pending, UnderReview
}

public enum IpApplicationType
{
    FirstApplication = 1,
    Renewal,
    Subsequent
}

public enum AppealType
{
    Administrative = 1,
    Judicial,
    InternationalReview
}

public enum AppealDecision
{
    Pending = 1,
    Upheld,
    Dismissed,
    Withdrawn
}

public enum RelationshipToFile
{
    PrimaryApplicant = 1,
    Spouse,
    Dependent,
    FamilyMember
}
