using Microsoft.Extensions.DependencyInjection;
using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Models;
using RECAMAS.Application.Interfaces;

namespace RECAMAS.Application.Grids.Services;

/// <summary>
/// Default <see cref="IGridQueryService"/> implementation. Resolves the registered
/// <see cref="IGridSourceProviderMarker"/> matching a grid key (case-insensitively) from the DI
/// container and dispatches the request to it.
/// </summary>
public sealed class GridQueryService : IGridQueryService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="GridQueryService"/>.
    /// </summary>
    /// <param name="dbContext">The database context passed to the resolved provider for data access.</param>
    /// <param name="serviceProvider">The service provider used to resolve registered grid source providers.</param>
    public GridQueryService(IApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<DataGridResponse<object>> QueryAsync(string gridKey, DataGridRequest request, CancellationToken ct = default)
    {
        var provider = GetProvider(gridKey)
            ?? throw new InvalidOperationException($"Grid '{gridKey}' not found");

        return await provider.ExecuteAsync(_dbContext, request, ct);
    }

    /// <inheritdoc/>
    public IGridSourceProviderMarker? GetProvider(string gridKey)
    {
        var providers = _serviceProvider.GetServices<IGridSourceProviderMarker>();
        return providers.FirstOrDefault(p => p.Key.Equals(gridKey, StringComparison.OrdinalIgnoreCase));
    }
}
