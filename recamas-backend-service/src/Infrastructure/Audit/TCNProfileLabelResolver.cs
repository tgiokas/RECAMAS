using Cbs.Audit.Abstractions;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Audit;

/// Verified against the real Cbs.Audit source (Cbs.Audit/Abstractions/IAuditLabelResolver.cs):
/// EntityType matches target.type, ResolveTargetLabel names the record, ResolveFieldLabel
/// renders an individual changed field (e.g. a foreign key) — TCNProfile has none needing
/// that yet, so it falls back to null (raw value already meaningful).
public class TCNProfileLabelResolver : IAuditLabelResolver
{
    public string EntityType => "TCNProfile";

    public string? ResolveTargetLabel(object entity)
    {
        if (entity is not TCNProfile profile)
        {
            return null;
        }

        return $"{profile.FirstNameEn} {profile.LastNameEn}".Trim();
    }

    public string? ResolveFieldLabel(string field, object? value) => null;
}
