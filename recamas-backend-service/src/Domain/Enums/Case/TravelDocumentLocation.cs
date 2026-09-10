namespace RECAMAS.Domain.Enums;

/// Location travel document στο counselling (§4.4.2.2 Section C — ταυτόσημο αλλά ξεχωριστό context)
public enum TravelDocumentLocation
{
    WithApplicant,
    AtAsylumService,
    WithAuthorities,
    Lost,
    Stolen,
    NeverHeld
}
