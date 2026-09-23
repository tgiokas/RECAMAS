using System.ComponentModel.DataAnnotations;

namespace Shared.Web.Contracts;

public sealed record DateOfBirthCriteria(DateOnly? ExactDate, DateOnly? StartDate, DateOnly? EndDate);

public sealed record StoplistSearchCriteria(
    [param: MaxLength(150)] string? FirstNames,
    [param: MaxLength(150)] string? LastName,
    DateOfBirthCriteria? DateOfBirth,
    string? Nationality,
    [param: MaxLength(50)] string? TravelDocumentNumber,
    string? TravelDocumentIssuingCountry,
    string? TravelDocumentType,
    string? Gender);

public sealed record ArrivalsDeparturesSearchCriteria(
    [param: MaxLength(150)] string? FirstNames,
    [param: MaxLength(150)] string? LastName,
    DateOfBirthCriteria? DateOfBirth,
    string? Nationality,
    [param: MaxLength(50)] string? TravelDocumentNumber,
    string? PassportIssuingCountry);

public sealed record StoplistResponse(bool HitFound);
public sealed record ArrivalsDeparturesResponse(IReadOnlyList<ArrivalResponse> Arrivals, IReadOnlyList<DepartureResponse> Departures);
public sealed record ArrivalResponse(long? ArrId, long? IndIndId, DateOnly? ArrivalDate, string? PassportIssueCountry,
    string? PassportNo, string? VisaNo, long? DepDepId, int? ArrivalStatus);
public sealed record DepartureResponse(long? DepId, long? IndIndId, DateOnly? DepartureDate, string? PassportIssueCountry,
    string? PassportNo, int? DepartureStatus);

public static class PoliceContractValidation
{
    public static string? Validate(DateOfBirthCriteria? dob, string? nationality, string? documentType = null, string? gender = null)
    {
        if (dob is not null)
        {
            if (dob.ExactDate.HasValue && (dob.StartDate.HasValue || dob.EndDate.HasValue))
                return "Provide either exactDate or a startDate/endDate range, not both.";
            if (dob.StartDate.HasValue != dob.EndDate.HasValue)
                return "Both startDate and endDate are required for a date range.";
            if (dob.StartDate > dob.EndDate)
                return "startDate must not be later than endDate.";
        }
        if (!string.IsNullOrWhiteSpace(nationality) && (nationality.Length != 3 || !nationality.All(char.IsLetter)))
            return "nationality must be an ISO 3166-1 alpha-3 code.";
        if (documentType is not null && documentType is not ("P" or "I"))
            return "travelDocumentType must be P or I.";
        if (gender is not null && gender is not ("M" or "F" or "U"))
            return "gender must be M, F or U.";
        return null;
    }
}
