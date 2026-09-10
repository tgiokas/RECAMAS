namespace RECAMAS.Domain.Enums;

/// Τύπος post-arrival assistance (§6.3.1.4.2)
public enum PostArrivalAssistanceType
{
    AirportAssistance,              // Βοήθεια στο αεροδρόμιο κατά άφιξη
    DomesticTicket,                 // Εισιτήριο εσωτερικής μεταφοράς
    Accommodation,                  // Στέγαση για ενδιάμεση στάση
    SpecialMeasures,                // Ειδικά μέτρα για ευάλωτες ομάδες
    MedicalCare
}
