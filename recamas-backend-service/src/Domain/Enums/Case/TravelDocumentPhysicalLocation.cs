namespace RECAMAS.Domain.Enums;

/// Physical location travel document (§4.4.2.3.3)
public enum TravelDocumentPhysicalLocation
{
    WithApplicant,
    AtAsylumService,
    WithAuthorities,
    Lost,
    Stolen,
    NeverHeld,
    AIUOffice                       // Παραδόθηκε στο A&IU office
}
