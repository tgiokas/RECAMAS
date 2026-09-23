using RECAMAS.Application.Dtos.Interoperability.ArrivalDeparture;
using RECAMAS.Application.Dtos.Interoperability.Shared;
using RECAMAS.Application.Dtos.Interoperability.StopList;
using RECAMAS.Application.Errors;
using RECAMAS.Application.Interfaces.Interoperability;
using RECAMAS.Application.Modules.TCNProfile;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Interfaces;

namespace RECAMAS.Application.Tests;

public sealed class TcnSearchOrchestratorTests
{
    [Fact]
    public async Task SearchAsync_PreservesSuccessfulResultsWhenCassFails()
    {
        var interoperability = new StubInteroperabilityService();
        var orchestrator = new TcnSearchOrchestrator(interoperability, new EmptyProfileRepository());
        var request = new ExternalSearchRequest("ARC-1", null, null, null, null, null);

        var result = await orchestrator.SearchAsync(request, new ExternalRequestContext("corr-3"));

        Assert.True(result.Ars.Success);
        Assert.False(result.Cass?.Success);
        Assert.True(result.Stoplist?.Success);
        Assert.True(result.ArrivalsDepartures?.Success);
        Assert.Equal("corr-3", result.CorrelationId);
    }

    private sealed class StubInteroperabilityService : IInteroperabilityService
    {
        public Task<ExternalServiceResult<TResponse>> ExecuteAsync<TResponse>(string serviceName, string operation, object request, ExternalRequestContext context, CancellationToken cancellationToken = default)
        {
            object? data = serviceName switch
            {
                "ARS" => new List<ExternalPersonDto> { new("ARS", "ARC-1", null, null, "Ada", "Lovelace", new DateOnly(1815, 12, 10), "GBR", "P-1") },
                "STOPLIST" => new StoplistSearchResult(false),
                "ARRIVALS_DEPARTURES" => new ArrivalsDeparturesSearchResult([], []),
                _ => null,
            };
            var success = serviceName != "CASS";
            return Task.FromResult(new ExternalServiceResult<TResponse>(
                success, serviceName, operation, success ? (TResponse?)data : default,
                context.CorrelationId, success ? null : ErrorCodes.Interoperability.Unavailable));
        }
    }

    private sealed class EmptyProfileRepository : ITcnProfileRepository
    {
        public Task<TcnProfile?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default) => Task.FromResult<TcnProfile?>(null);
        public Task<TcnProfile?> GetByPublicIdWithDetailsAsync(Guid publicId, CancellationToken cancellationToken = default) => Task.FromResult<TcnProfile?>(null);
        public Task<TcnProfile?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default) => Task.FromResult<TcnProfile?>(null);
        public Task<TcnProfile?> GetByArcAsync(string arc, CancellationToken cancellationToken = default) => Task.FromResult<TcnProfile?>(null);
        public Task<IReadOnlyList<TcnProfile>> SearchForDuplicatesAsync(string? arc, string? passportNumber, string? firstName, string? lastName, DateOnly? dateOfBirth, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<TcnProfile>>([]);
        public Task<(IReadOnlyList<TcnProfile> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? quickSearchTerm, CancellationToken cancellationToken = default) => Task.FromResult<(IReadOnlyList<TcnProfile>, int)>(([], 0));
        public Task AddAsync(TcnProfile profile, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
