# TCNProfile module

Search/dedupe/merge TCN identities. `TcnSearchOrchestrator` invokes the in-process
Interoperability dispatcher, which owns the adapters to ARS, CASS, Stoplist and
Arrivals/Departures. This module never calls external transport clients directly.

Implemented: TCNProfileService, external TCN search and duplicate candidates.
Planned: explicit ProfileMergeCommand. Schema: tcn_profile.
