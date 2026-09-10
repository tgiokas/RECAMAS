namespace RECAMAS.Domain.Enums;

/// Λόγος επιστροφής (§4.4.2.1)
public enum ReturnReason
{
    ILMigrant,                      // Παράτυπος μετανάστης
    AsylumSeeker,
    AsylumRejection,
    ExpiredResidencePermit,
    RevocationResidencePermit,
    Student
}
