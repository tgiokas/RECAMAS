namespace RECAMAS.Domain.Enums;

/// Status profile TCN (§3.4)
public enum TcnProfileStatus
{
    AVRApplicationPending,          // Ενεργό case σε Counselling / ApplicationProcessing / Evaluation
    AVRReturnPending,               // Πέρασε Evaluation, αναμένει αναχώρηση
    Departed,                       // Αναχώρηση επιβεβαιώθηκε από Arrivals/Departures
    ForcedReturnPreliminaryDetention, // Ενεργό FRC σε Preliminary Detention
    ForcedReturnPending,            // FRC initiated, δεν έχει αναχωρήσει
    ByOwnReturnPending,             // By Own initiated, αναμένει αναχώρηση
    Dismissed                       // Τελευταίο case ακυρώθηκε
}
