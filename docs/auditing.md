# Auditing (Cbs.Audit)

RECAMAS's business audit trail uses the `Cbs.Audit` /
`Cbs.Audit.AspNetCore` package (private feed, `cosmos-business-systems/cbs-audit`).
This project only wires it up and annotates entities; the capture/outbox/relay
mechanism itself lives in the package.

Cbs.Audit answers "what did a user do" and ends up in Elasticsearch (`recamas-audit-events`
index by default).

## How it works

```
[Audited] entity change          IAuditRecorder.RecordAsync(...)
        │                                    │
        ▼                                    ▼
  AuditSaveChangesInterceptor  ──────►  audit_outbox table (Postgres)
   (wired in InfrastructureServiceRegistration.cs)     │
                                                        ▼
                                              AuditRelay (background service)
                                                        │
                                                        ▼
                                                  Elasticsearch
```

Two ways an event gets written — same outbox table, same delivery path either way:

1. **Automatic** — any entity marked `[Audited]` gets its Created/Updated/Deleted
   diff captured for free, every time `SaveChangesAsync()` runs. No code to write.
2. **Explicit** — for anything that isn't a row change (login, export, a bulk
   stored-proc update), inject `IAuditRecorder` and call `RecordAsync(...)`.

If Elasticsearch is down, events just sit in `audit_outbox` and the relay retries
with backoff — nothing is lost, nothing blocks the request.

## How to audit a new entity

1. Add `[Audited(Type = "YourEntity", BusinessKey = nameof(SomeNaturalKey))]`
   to the class (see `TCNProfile.cs` for a real example).
2. Mask or exclude sensitive fields:
   - `[MaskedAudit(Mask.Full)]` (or `Mask.Email`, `Mask.Phone`, etc.) — records
     *that* the field changed, not the value. **Use this for sensitive data.**
   - `[NotAudited]` — the field never appears in a diff at all. Only for
     bulky/meaningless columns, not sensitive ones (masking is the right tool
     there, per the package's own guidance).
3. Add the entity's action codes to `src/API/audit/actions.yaml`:
   ```yaml
   - code: YOURENTITY.CREATED
     category: Your Module
     severity: high
     requiresTarget: true
     retention: legal   # or "standard" — legal = 7 years, standard = 2 years
   - code: YOURENTITY.UPDATED
     ...
   - code: YOURENTITY.DELETED
     ...
   ```
   The prefix is `Type` upper-cased (`Type = "TCNProfile"` → `TCNPROFILE.*`).
   **Startup fails** if an `[Audited]` entity would emit a code missing from
   this file — you'll find out immediately, not in production.
4. (Optional) If raw foreign-key/ID values in the diff aren't meaningful to a
   reader, add a resolver implementing `IAuditLabelResolver` and register it
   in `Program.cs` via `.AddLabelResolver<T>()` — see `TCNProfileLabelResolver.cs`.

## How to record a non-entity action

```csharp
public class SomeService(IAuditRecorder audit)
{
    public async Task ExportReportAsync(...)
    {
        // ... do the work ...
        await audit.RecordAsync(
            action: "REPORT.EXPORTED",
            target: new AuditTarget { Type = "Report", Id = reportId });
    }
}
```
Add `REPORT.EXPORTED` to `actions.yaml` first, same rules as above.

## Things to know

- **Action codes are append-only.** Never rename or reuse one — old events in
  Elasticsearch still reference it.
- `audit_outbox` lives in the default (`public`) schema, unlike every other table
  in this project — the package's own table-mapping call takes no schema parameter.
- Retention (`standard` = 2 years, `legal` = 7 years) and masking level are policy
  decisions, not just plumbing — confirm with the team before shipping a new
  entity's audit config, don't just copy `TCNProfile`'s choices by default.
