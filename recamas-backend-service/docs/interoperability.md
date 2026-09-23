# Interoperability

RECAMAS hosts interoperability as an in-process Application module. There is no
separate interoperability API, database, API key, or Docker service.

## Runtime flow

`POST /api/tcn-search` validates the search request and invokes ARS first. When
ARS returns a candidate, local duplicate detection runs and CASS, Stoplist, and
Arrivals/Departures execute concurrently. Each external result carries its own
success/error state, so downstream partial failures do not discard valid data.

The dispatcher is generic internally, but it is intentionally not exposed as a
generic public HTTP endpoint. Application callers use normalized typed contracts.

## Adapters

- `ARS`: implements the supplied ARS OpenAPI routes under
  `/api/v1/Alien/immigration-applicants` and sends `api-Key` plus correlation ID.
- `STOPLIST`: implements Police API v1.0.3 at
  `/police/police-checks/v1/stoplist/search`.
- `ARRIVALS_DEPARTURES`: implements Police API v1.0.2 at
  `/police/police-checks/v1/arrivals-departures/search`.
- `CASS`: remains an explicit development mock until the provider supplies its
  WSDL/XSD. With `CASS_USE_MOCK=false`, it returns an unavailable result.

## Configuration

Required environment keys are documented in the repository `.env.example`:

- `ARS_BASE_URL` (falls back to `CYCONNECT_BASE_URL`) and `ARS_API_KEY`
- `POLICE_BASE_URL`, `POLICE_API_KEY`, and `POLICE_CLIENT_ID`
- `CASS_USE_MOCK` (development only)

## Auditing and data ownership

Every adapter invocation writes an `admin.interface_sync_logs` record containing
correlation, timing, outcome, and SHA-256 payload hashes. Raw identity payloads
are not persisted in technical audit records.

External search is read-only. Adapters normalize transport data but do not create,
update, or merge TCN profiles. Those actions require an explicit TCN Profile
application command and business audit.
