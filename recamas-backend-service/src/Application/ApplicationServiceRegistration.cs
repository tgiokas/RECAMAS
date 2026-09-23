using Microsoft.Extensions.DependencyInjection;

using FluentValidation;

using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Services;
using RECAMAS.Application.Interfaces;
using RECAMAS.Application.Interfaces.Interoperability;
using RECAMAS.Application.Modules;
using RECAMAS.Application.Modules.Interoperability;
using RECAMAS.Application.Modules.TCNProfile;

namespace RECAMAS.Application;

/// Registers every Application-layer service, across every module, in one place.
/// Called once from API/Program.cs as services.AddApplicationServices().
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServiceRegistration));

        // --- Generic server-side data grid framework (Application/Grids) ---
        // Concrete grids are registered as IGridSourceProviderMarker per module as they are built.
        services.AddScoped<IGridQueryService, GridQueryService>();

        // --- TCNProfile module ---
        services.AddScoped<ITCNProfileService, TCNProfileService>();
        services.AddScoped<ITcnSearchOrchestrator, TcnSearchOrchestrator>();
        services.AddScoped<IInteroperabilityService, ExternalServiceDispatcher>();

        // --- Case module ---
        // services.AddScoped<ICaseService, CaseService>();

        // --- Detention module ---
        // services.AddScoped<IDetentionService, DetentionService>();

        // --- ReturnImplementation module ---
        // services.AddScoped<IReturnImplementationService, ReturnImplementationService>();

        // --- Rules module (in-process, no HTTP client — see Domain/Interfaces/IRuleEvaluator) ---
        // services.AddScoped<IRuleEvaluator, RuleEvaluator>();
        // services.AddScoped<IRuleService, RuleService>();

        // --- Reports module ---
        // services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
