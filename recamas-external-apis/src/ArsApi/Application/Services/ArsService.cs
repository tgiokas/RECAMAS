using ArsApi.Application.DTOs;
using ArsApi.Application.Interfaces;
using ArsApi.Domain.Entities;

namespace ArsApi.Application.Services;

public class ArsService(IArsRepository repo)
{
    public async Task<IEnumerable<ArsSearchResponse>> SearchAsync(
        ArsSearchRequest req, CancellationToken ct = default)
    {
        var records = await repo.SearchAsync(
            req.Arc, req.Name, req.Surname, req.Nationality,
            req.PassportNo, req.DateOfBirth, req.MdFileNumber, ct);

        return records.Select(MapToResponse);
    }

    public async Task<ArsSearchResponse> CreateAsync(
        ArsCreateRequest req, CancellationToken ct = default)
    {
        var entity = new ArsRecord
        {
            Arc = req.Arc,
            FirstName = req.FirstName,
            LastName = req.LastName,
            Nationality = req.Nationality,
            Gender = req.Gender,
            PassportNo = req.PassportNo,
            PassportExpirationDate = req.PassportExpirationDate,
            DateOfBirth = req.DateOfBirth,
            PlaceOfBirth = req.PlaceOfBirth,
            Address = req.Address,
            PhoneNo = req.PhoneNo,
            MdFileNo = req.MdFileNo,
            RelationshipToFile = req.RelationshipToFile,
            ArsFolderNumber = req.ArsFolderNumber,
            PermitType = req.PermitType,
            ResidencyIssueDate = req.ResidencyIssueDate,
            ResidenceCategory = req.ResidenceCategory,
            PurposeOfResidence = req.PurposeOfResidence,
            ResidencyExpiryDate = req.ResidencyExpiryDate,
            ResidencyStatus = req.ResidencyStatus,
            ResidencyDocumentNumber = req.ResidencyDocumentNumber,
            TypeOfPermitRequested = req.TypeOfPermitRequested,
            TypeOfApplication = req.TypeOfApplication,
            ApplicationSubmissionDate = req.ApplicationSubmissionDate,
            ApplicationResidenceCategory = req.ApplicationResidenceCategory,
            ApplicationPurposeOfResidence = req.ApplicationPurposeOfResidence,
            ApplicationDecisionDate = req.ApplicationDecisionDate,
            ApplicationStatus = req.ApplicationStatus,
            ReturnDecisionDate = req.ReturnDecisionDate,
            ReturnDecisionText = req.ReturnDecisionText,
            VoluntaryReturnDeadlineDays = req.VoluntaryReturnDeadlineDays,
            EntryBanDurationMonths = req.EntryBanDurationMonths
        };

        var created = await repo.CreateAsync(entity, ct);
        return MapToResponse(created);
    }

    /// <summary>
    /// Finds sets of records that share an identifying attribute (surname,
    /// date of birth, surname+country of origin, or travel document issuing
    /// country) and may therefore represent the same or a related TCN.
    /// </summary>
    public async Task<IEnumerable<ArsDuplicateGroup>> GetPotentialDuplicatesAsync(CancellationToken ct = default)
    {
        var records = (await repo.GetAllAsync(ct)).ToList();
        var groups = new List<ArsDuplicateGroup>();

        groups.AddRange(records
            .GroupBy(r => r.LastName.Trim().ToLowerInvariant())
            .Where(g => g.Count() > 1)
            .Select(g => new ArsDuplicateGroup(
                ArsDuplicateMatchType.SameLastName, g.Key,
                g.Select(MapToResponse).ToList())));

        groups.AddRange(records
            .GroupBy(r => r.DateOfBirth.Date)
            .Where(g => g.Count() > 1)
            .Select(g => new ArsDuplicateGroup(
                ArsDuplicateMatchType.SameDateOfBirth, g.Key.ToString("yyyy-MM-dd"),
                g.Select(MapToResponse).ToList())));

        groups.AddRange(records
            .GroupBy(r => $"{r.LastName.Trim().ToLowerInvariant()}|{r.Nationality}")
            .Where(g => g.Count() > 1)
            .Select(g => new ArsDuplicateGroup(
                ArsDuplicateMatchType.SameLastNameAndCountryOfOrigin, g.Key,
                g.Select(MapToResponse).ToList())));

        groups.AddRange(records
            .GroupBy(r => r.Nationality)
            .Where(g => g.Count() > 1)
            .Select(g => new ArsDuplicateGroup(
                ArsDuplicateMatchType.SameTravelDocumentIssuingCountry, g.Key.ToString(),
                g.Select(MapToResponse).ToList())));

        return groups;
    }

    /// <summary>
    /// Groups records that share the same ARS folder number, simulating
    /// members of the same family / case file (RECAMAS §2.2.4 Linked Profile Suggestion).
    /// </summary>
    public async Task<IEnumerable<ArsFolderGroup>> GetFolderGroupsAsync(CancellationToken ct = default)
    {
        var records = await repo.GetAllAsync(ct);

        return records
            .Where(r => !string.IsNullOrWhiteSpace(r.ArsFolderNumber))
            .GroupBy(r => r.ArsFolderNumber!)
            .Where(g => g.Count() > 1)
            .Select(g => new ArsFolderGroup(g.Key, g.Select(MapToResponse).ToList()));
    }

    private static ArsSearchResponse MapToResponse(ArsRecord r) => new(
        r.Arc, r.FirstName, r.LastName, r.Nationality, r.Gender,
        r.PassportNo, r.PassportExpirationDate, r.DateOfBirth,
        r.PlaceOfBirth, r.Address, r.PhoneNo, r.MdFileNo, r.RelationshipToFile,
        r.ArsFolderNumber,
        r.PermitType == null ? null : new ArsResidencyStatusDto(
            r.PermitType, r.ResidencyIssueDate, r.ResidenceCategory,
            r.PurposeOfResidence, r.ResidencyExpiryDate,
            r.ResidencyStatus, r.ResidencyDocumentNumber),
        r.TypeOfPermitRequested == null ? null : new ArsResidencyApplicationDto(
            r.TypeOfPermitRequested, r.TypeOfApplication,
            r.ApplicationSubmissionDate, r.ApplicationResidenceCategory,
            r.ApplicationPurposeOfResidence, r.ApplicationDecisionDate,
            r.ApplicationStatus),
        r.ReturnDecisionDate == null ? null : new ArsReturnDecisionDto(
            r.ReturnDecisionDate, r.ReturnDecisionText,
            r.VoluntaryReturnDeadlineDays, r.EntryBanDurationMonths)
    );
}
