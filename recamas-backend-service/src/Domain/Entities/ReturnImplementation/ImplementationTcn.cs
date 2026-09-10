using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Per-TCN details εντός implementation (§6.3.1.2)
public class ImplementationTcn : BaseEntity
{
    public long ReturnImplementationId { get; set; }
    public long TcnProfileId { get; set; }
    public long CaseId { get; set; }

    // Ticket (§6.3.1.2.3.1)
    public string? TravelAgency { get; set; }           // Free text
    public decimal? TicketCost { get; set; }
    public string? Airline { get; set; }                // Free text (airline name)
    public string? FlightNumber { get; set; }           // Free text (airline format)
    public DateOnly? DepartureDate { get; set; }
    public string? DepartureAirportCode { get; set; }   // IATA code — string (standard)
    public string? DestinationCountryCode { get; set; } // ISO 3166-1 alpha-2 — string
    public string? DestinationAirportCode { get; set; } // IATA code — string
    public string? TicketNumber { get; set; }           // Free text (airline format)
    public string? TicketFilePath { get; set; }
    public string? InvoiceFilePath { get; set; }

    // Agreement and Final Confirmation (§6.3.1.2.3.2)
    public bool TravelDocumentReceipt { get; set; }
    public bool ReturnDecisionAccepted { get; set; }
    public string? SignedReturnDecisionPath { get; set; }
    public bool TicketProvided { get; set; }

    // Departure Details (§6.3.1.2.3.3)
    public bool MonetaryIncentiveReceived { get; set; }
    public string? MonetaryIncentiveReceiptPath { get; set; }
    public string? DepartureDetailsAttachmentsPath { get; set; }

    // Departure Confirmation (§6.3.1.2.3.4)
    public bool Departed { get; set; }
    public DateOnly? DepartureConfirmationDate { get; set; }

    // Entry Ban (§6.3.1.2.3.5)
    public int? EntryBanDurationMonths { get; set; }
    public DateOnly? EntryBanExpirationDate { get; set; } // Calculated
    public bool StoplistConfirmation { get; set; }
    public string? UniqueEntryBanNumber { get; set; }   // Από Stoplist — free text (Police DB format)

    // Monetary Incentive Forced (§6.3.2.2.3.2)
    public decimal? MonetaryIncentiveApprovedAmount { get; set; }
    public bool? MonetaryIncentiveProvided { get; set; }
    public decimal? MonetaryIncentiveAmountProvided { get; set; }
    public string? MonetaryIncentiveNotes { get; set; }
    public string? MonetaryIncentiveAttachmentPath { get; set; }

    // Post Arrival Assistance (§6.3.1.4)
    public bool PostArrivalMonetaryIncentiveProvided { get; set; }
    public DateOnly? AssistanceDeadline { get; set; }   // Calculated: DepartureDate + 5 months
    public bool PostArrivalEligible { get; set; }

    // ByOwn Agreement
    public bool? ByOwnReturnDecisionAccepted { get; set; }
    public string? ByOwnSignedReturnDecisionPath { get; set; }

    public ICollection<PostArrivalAssistance> PostArrivalAssistances { get; set; } = [];

    public ReturnImplementation ReturnImplementation { get; set; } = null!;
    public TcnProfile TcnProfile { get; set; } = null!;
    public ReturnCase Case { get; set; } = null!;
}
