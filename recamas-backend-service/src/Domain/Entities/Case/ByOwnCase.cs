using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Voluntary Return By Own Means case (§4.6)
public class ByOwnCase : ReturnCase
{
    public ICollection<ByOwnReturnDecisionIssuance> ReturnDecisionIssuances { get; set; } = [];
}
