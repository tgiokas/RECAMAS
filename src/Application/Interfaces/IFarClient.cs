namespace RECAMAS.Application.Interfaces;

/// FRONTEX's FAR system. Study 9.7/12.3.8: "the technical specifications and
/// available APIs for FAR have not yet been finalized" — this interface is
/// deliberately just a hook (mirrors architecture diagram's "FAR (FRONTEX)"
/// box and "hooks" note), not a real contract. No methods until Frontex
/// publishes the API, expected as part of IRMA 2.0.
public interface IFarClient
{
}
