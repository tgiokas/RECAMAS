# Interoperability module

This module is the in-process anti-corruption layer for external RECAMAS systems.
Application workflows call `IInteroperabilityService`; they must not inject an
ARS, CASS, Stoplist, or Arrivals/Departures transport client directly.

`ExternalServiceDispatcher` resolves an `IExternalServiceAdapter` by its stable,
case-insensitive service name and wraps every call in technical integration
auditing. Adapters and external wire contracts live in Infrastructure. The
normalized request/results in Application are the only contracts visible to
business workflows.

The TCN search sequence is synchronous: ARS first, then local duplicate lookup,
then CASS, Stoplist, and Arrivals/Departures concurrently. A downstream failure
is returned as a per-service result and does not discard other successful data.

Search is read-only. Persisting or merging a selected external candidate requires
an explicit TCN Profile command so external responses never silently overwrite
RECAMAS-owned values.
