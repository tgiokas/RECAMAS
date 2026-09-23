using RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Dtos.Interoperability.StopList;
using RECAMAS.Application.Interfaces.Interoperability;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Application.Modules.TCNProfile;

public sealed class TcnSearchOrchestrator(
    IInteroperabilityService interoperability,
    ITcnProfileRepository profiles) : ITcnSearchOrchestrator
{
    public async Task<TcnSearchResult> SearchAsync(
        ExternalSearchRequest request,
        ExternalRequestContext context,
        CancellationToken cancellationToken = default)
    {
        var ars = await interoperability.ExecuteAsync<IReadOnlyList<ExternalPersonDto>>(
            "ARS", "Search", request, context, cancellationToken);

        var selected = ars.Data?.FirstOrDefault();
        if (!ars.Success || selected is null)
            return new(ars, null, null, null, [], context.CorrelationId);

        var enrichedPerson = selected with
        {
            Arc = selected.Arc ?? request.Arc,
            PassportNumber = selected.PassportNumber ?? request.PassportNumber,
            PassportIssuingCountry = selected.PassportIssuingCountry ?? request.PassportIssuingCountry,
            TravelDocumentType = selected.TravelDocumentType ?? request.TravelDocumentType,
            Gender = selected.Gender ?? request.Gender,
        };

        var duplicates = await profiles.SearchForDuplicatesAsync(
            enrichedPerson.Arc,
            enrichedPerson.PassportNumber,
            enrichedPerson.FirstName,
            enrichedPerson.LastName,
            enrichedPerson.DateOfBirth,
            cancellationToken);

        var enrichedRequest = request with
        {
            Arc = enrichedPerson.Arc,
            PassportNumber = enrichedPerson.PassportNumber,
            PassportIssuingCountry = enrichedPerson.PassportIssuingCountry,
            TravelDocumentType = enrichedPerson.TravelDocumentType,
            Gender = enrichedPerson.Gender,
        };

        var cassTask = interoperability.ExecuteAsync<ExternalPersonDto?>(
            "CASS", "Search", enrichedPerson, context, cancellationToken);
        var stoplistTask = interoperability.ExecuteAsync<StoplistSearchResult>(
            "STOPLIST", "Search", enrichedRequest, context, cancellationToken);
        var movementsTask = interoperability.ExecuteAsync<ArrivalsDeparturesSearchResult>(
            "ARRIVALS_DEPARTURES", "Search", enrichedRequest, context, cancellationToken);

        await Task.WhenAll(cassTask, stoplistTask, movementsTask);

        var candidates = duplicates.Select(x => new DuplicateCandidateDto(
            x.PublicId, x.RecamasId, x.Arc, x.FirstNameEn, x.LastNameEn, x.DateOfBirth)).ToList();

        return new(
            ars,
            await cassTask,
            await stoplistTask,
            await movementsTask,
            candidates,
            context.CorrelationId);
    }
}
