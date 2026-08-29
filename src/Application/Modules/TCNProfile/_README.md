# TCNProfile module

Search/dedupe/merge TCN identities; owns adapters to ARS, CASS, Stoplist,
Arrivals/Departures (called from Infrastructure/ExternalClients, orchestrated here).

Planned: TCNProfileService, ProfileSearchDto, ProfileMergeCommand,
DuplicateCandidateDto. Schema: tcn_profile.
