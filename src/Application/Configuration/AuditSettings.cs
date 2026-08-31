using Microsoft.Extensions.Configuration;

namespace RECAMAS.Application.Configuration;

/// Backs the AddCbsAudit(...) call in Program.cs. Flat env keys, same
/// convention as every other *Settings class here — RECAMAS_/CIVILIANPORTAL_-style
/// inline builder.Configuration[...] reads (the doc's own example) were
/// deliberately not copied, to stay consistent with this project's .env conversion.
public class AuditSettings
{
    public string ElasticsearchUri { get; init; } = "http://elasticsearch:9200";
    public string Index { get; init; } = "recamas-audit-events";
    public string? Env { get; init; }
    public bool RelayEnabled { get; init; } = true;
    public int? RelayMaxAttempts { get; init; }
    public int? OutboxKeepDays { get; init; }

    /// "none" / "initials" / "full" / "partial" (default) — see Program.cs for the Mask mapping.
    public string ActorMask { get; init; } = "partial";

    public static AuditSettings BindFromConfiguration(IConfiguration configuration)
    {
        return new AuditSettings
        {
            ElasticsearchUri = configuration["ELASTICSEARCH_URI"] ?? "http://elasticsearch:9200",
            Index = configuration["AUDIT_INDEX"] ?? "recamas-audit-events",
            Env = configuration["AUDIT_ENV"],
            RelayEnabled = configuration["AUDIT_RELAY_ENABLED"] is not "false",
            RelayMaxAttempts = int.TryParse(configuration["AUDIT_RELAY_MAX_ATTEMPTS"], out var maxAttempts) ? maxAttempts : null,
            OutboxKeepDays = int.TryParse(configuration["AUDIT_OUTBOX_KEEP_DAYS"], out var keepDays) ? keepDays : null,
            ActorMask = configuration["AUDIT_ACTOR_MASK"] ?? "partial",
        };
    }
}
