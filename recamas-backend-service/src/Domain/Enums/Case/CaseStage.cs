namespace RECAMAS.Domain.Enums;

/// Stage case (§4.4.1 / §4.5.1 / §4.6.1)
public enum CaseStage
{
    Counselling,                    // AVR
    ApplicationProcessing,          // AVR
    Evaluation,                     // AVR
    PreReturn,                      // AVR / FRC
    ReturnImplementation,           // AVR / FRC
    Reintegration,                  // AVR
    PreliminaryDetention,           // FRC
    Detention,                      // FRC
    CaseProcessing,                 // ByOwn
    Implementation                  // ByOwn
}
