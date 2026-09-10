namespace RECAMAS.Domain.Enums;

/// Τύπος alternative measure FRC (§4.5.2.4.2)
public enum AlternativeMeasureType
{
    ReportingToAuthorities,         // Υποχρεωτική εμφάνιση
    ResidenceRequirement,           // Υποχρέωση διαμονής σε συγκεκριμένο χώρο
    SurrenderPassport,              // Παράδοση ταξιδιωτικού εγγράφου
    ResidenceAddressObligation,     // Υποχρέωση γνωστοποίησης διεύθυνσης
    ReleaseOnBail,
    CommunityManagement,
    CompulsoryReturnCounselling,
    TechnologyBased,                // Ηλεκτρονική παρακολούθηση
    Other
}
