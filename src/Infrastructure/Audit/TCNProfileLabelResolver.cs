using Cbs.Audit.Abstractions;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Infrastructure.Audit;

/// UNVERIFIED SHAPE: the adc6c866-auditing.md doc only describes what
/// IAuditLabelResolver implementations do (EntityType property, builds a label
/// from the in-memory entity only — resolvers run mid-SaveChanges, so this must
/// NOT touch the database), not the interface's own member names/signature or
/// which Cbs.Audit.* package declares it. Confirm both against the real
/// package before trusting this compiles as written.
public class TCNProfileLabelResolver : IAuditLabelResolver
{
    public string EntityType => "TCNProfile";

    public string? Resolve(object entity)
    {
        if (entity is not TCNProfileEntity profile)
        {
            return null;
        }

        return $"{profile.FirstNameEn} {profile.LastNameEn}".Trim();
    }
}
