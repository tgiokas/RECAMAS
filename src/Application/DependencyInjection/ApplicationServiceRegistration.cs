using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RECAMAS.Application.DependencyInjection;

/// <summary>
/// Registers every Application-layer service, across every module, in one place.
/// Called once from API/Program.cs as services.AddApplicationServices().
/// As each module gets built, add its service registrations here —
/// e.g. services.AddScoped&lt;ICaseService, CaseService&gt();
/// Do NOT hand-wire these directly in Program.cs.
/// </summary>
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServiceRegistration));

        // --- TCNProfile module ---
        // services.AddScoped<ITCNProfileService, TCNProfileService>();

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
