namespace RECAMAS.Domain.Entities.Case;

/// Voluntary Return By Own Means case (§4.6)
public class ByOwnCase : ReturnCase
{
    public ICollection<ByOwnReturnDecisionIssuance> ReturnDecisionIssuances { get; set; } = [];
}
