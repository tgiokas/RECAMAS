# Grids (server-side data grid framework)

A generic framework for serving **paged, sorted, filtered, searched, grouped and summarised**
tabular data to UI grids (built around DevExtreme DataGrid's `loadOptions`, but usable with any
client). One controller endpoint serves every grid; each grid is a small, declarative plug-in
identified by a **grid key**.

- Endpoint: `POST /api/grids/{gridKey}` (`src/API/Controllers/GridsController.cs`)
- Framework code: `src/Application/Grids/`
- Concrete grids: configuration + DTO in `src/Application/Grids/<Name>/`, provider in
  `src/Infrastructure/Grids/<Name>/`
- Currently registered grids: `tcnProfiles`

## How it works

```
POST /api/grids/tcnProfiles  { DataGridRequest }
        │
        ▼
GridsController ── unknown key ──► 404
        │
        ▼
GridQueryService.GetProvider(key)   finds the IGridSourceProviderMarker whose Key matches
        │                           (case-insensitive) among all DI-registered providers
        ▼
GridSourceProvider<TDto>.ExecuteAsync
   1. loadOptions mapping     DevExtreme loadOptions → DataGridRequest (only if loadOptions sent)
   2. base query              provider's projection, e.g. from profile in Set<TcnProfile>() select new Dto {...}
   3. ApplyRequestQueryTransforms   (override hook, e.g. row scoping)
   4. filter → search         translated to LINQ expression trees (GridExpressionBuilder)
   5a. no group  → count, total summaries, sort (or default sort), Skip/Take, HydratePage
   5b. group     → grouped response (see "Grouping")
        │
        ▼
DataGridResponse { data, totalCount, groupCount, summary, meta }
```

Everything is evaluated on top of an `IQueryable`, so filtering/sorting/paging/counting run in the
database for the normal (ungrouped) path.

## Using a grid (API)

### Request

`POST /api/grids/{gridKey}` with a JSON body (`DataGridRequest`). Property names are
case-insensitive on input; responses are camelCase and enums are serialised as their **string names**.

| Property | Type | Default | Meaning |
|---|---|---|---|
| `skip` | int | `0` | Rows to skip (for grouped requests: groups to skip). Negative becomes 0. |
| `take` | int | `20` | Page size. `<= 0` becomes 20. Capped at the grid's `MaxPageSize` and the global cap of **1000**. |
| `sort` | `[{ field, desc }]` | `null` | Multi-level sort. When empty, the grid's default sort is used. |
| `filter` | filter tree | `null` | See "Filters". |
| `searchValue` | string | `null` | Free-text search value. |
| `searchExpr` | string[] | `null` | Fields to search. `null` = all `searchable` fields. |
| `searchOperation` | string | `"contains"` | `contains`, `startswith`, `endswith`, `=`/`eq`, `!=`/`neq`. Unknown values fall back to `contains`. |
| `group` | `[{ selector, desc, isExpanded, groupInterval }]` | `null` | See "Grouping". |
| `totalSummary` | `[{ selector, summaryType }]` | `null` | Aggregates over the whole filtered set. |
| `groupSummary` | `[{ selector, summaryType }]` | `null` | Aggregates per group. |
| `requireTotalCount` | bool | `true` | Compute `totalCount` (an extra `COUNT` query). |
| `loadOptions` | object | `null` | Raw DevExtreme `loadOptions`; overrides the properties above. See "DevExtreme integration". |

`requireGroupCount`, `select` and `userData` are accepted but currently **not used** by the pipeline.

### Response

```json
{
  "data": [ { "id": 12, "recamasId": "…", "status": "Departed", "…": "…" } ],
  "totalCount": 348,
  "groupCount": null,
  "summary": null,
  "meta": null
}
```

| Property | Meaning |
|---|---|
| `data` | Page of rows, or a page of groups when grouping (see below). |
| `totalCount` | Rows matching filter/search before paging. `0` when `requireTotalCount` is `false`. |
| `groupCount` | Number of top-level groups when grouping, otherwise `null`. |
| `summary` | Total summary **values**, in request order (composite keys are not returned), or `null`. Summaries that cannot be computed are omitted, so positions can shift. |
| `meta` | Optional provider-specific metadata (`BuildResponseMetadata`), usually `null`. |

Errors: unknown `gridKey` → `404`.

### Example: plain request

```http
POST /api/grids/tcnProfiles
Content-Type: application/json

{
  "skip": 0,
  "take": 25,
  "sort": [{ "field": "lastNameEn", "desc": false }],
  "searchValue": "papa",
  "searchExpr": ["lastNameEn", "lastNameEl"],
  "filter": {
    "type": "group",
    "operator": "and",
    "children": [
      { "type": "condition", "field": "status",    "operator": "eq", "value": "Departed" },
      { "type": "condition", "field": "flagMinor", "operator": "eq", "value": true }
    ]
  }
}
```

### Filters

A filter is a tree of **groups** (`and` / `or`) and **conditions**. The `filter` property accepts two
shapes, both normalised by `FilterNodeJsonConverter`:

**1. Internal object format** (explicit `type` discriminator)

```json
{ "type": "group", "operator": "or", "children": [
    { "type": "condition", "field": "arc", "operator": "startswith", "value": "AB" },
    { "type": "condition", "field": "createdAt", "operator": "between", "value": ["2025-01-01", "2025-01-31"] }
] }
```

**2. DevExtreme array format**

```json
[["status", "=", "Departed"], "and", [["flagMinor", "=", true], "or", ["flagNoArc", "=", true]]]
```

Also supported: negation `["!", [...]]`, and the header-filter operators `anyof` / `noneof`
(expanded to `or`-of-`eq` / `and`-of-`neq`).

Operators (case-insensitive):

| Operator | DevExtreme symbol | Notes |
|---|---|---|
| `eq`, `neq` | `=`, `<>` / `!=` | Case-insensitive for strings. |
| `gt`, `gte`, `lt`, `lte` | `>`, `>=`, `<`, `<=` | |
| `between` | - | `value` is `[low, high]`, `{ "from", "to" }` or `{ "low", "high" }`. Inclusive. |
| `contains`, `notcontains` | same | Case-insensitive. |
| `startswith`, `notstartswith` | `startswith` | Case-insensitive. |
| `endswith`, `notendswith` | `endswith` | Case-insensitive. |
| `isnull`, `notnull` | - | |
| `isempty`, `notempty` | - | Compares to `""`. |

Value handling worth knowing:

- Values are coerced to the field's CLR type: `bool`, `int`, `long`, `decimal`, `double`, `float`,
  `DateTime`, enums.
- **Enum fields** accept the numeric value, the member name (`"Departed"`), or the member's
  `[Display]`/`[Description]` text.
- **Dates**: strings without a zone are treated as UTC. A date-only value (`yyyy-MM-dd`) with `lt`
  is bumped by one day (so the whole day is included), and a `between` whose bounds are both
  midnight becomes `[low, high + 1 day)`. Values containing `NaN` (DevExtreme placeholders) are skipped.
- In a group, any operator other than `and` is treated as `or`.
- **Unknown fields, non-`filterable` fields, unknown operators and unparseable values are silently
  ignored.** The condition is dropped and the rest of the query still runs, so a typo in a field name
  returns *more* rows, not an error.
- String fields in the TCN grid are projected with `?? ""`, so use `isempty`/`notempty` rather than
  `isnull`/`notnull` on them.

### Search

`searchValue` builds an OR of `searchOperation` over the chosen fields (case-insensitive) and is
AND-ed with the filter. Only fields declared `searchable` participate. `searchExpr` narrows the list;
names that aren't searchable are dropped, and if none remain, no search is applied.

### Sorting and paging

- Sort fields must be declared `sortable`; others are skipped. Sort is multi-level.
- With no usable sort, the grid's `DefaultSort` is applied. If a grid has neither, paging is
  unordered, so always declare a default sort.
- Sorting is **not applied** to grouped responses (groups are ordered by key, see `group.desc`).

### Grouping

`group` is a list of levels: `{ "selector": "status", "desc": false, "isExpanded": true, "groupInterval": null }`.
`groupInterval` (`year` / `month` / `day`) buckets `DateTime` fields. Groups are returned as
`{ key, items, count, summary }`; `skip`/`take` page the **top-level groups**.

There are two execution paths:

| | Fast path | Full path |
|---|---|---|
| When | exactly **one** group level and **no** summaries | multiple levels, or any `totalSummary` / `groupSummary` |
| How | only the group key column is fetched, then grouped and counted | **all** matching rows are loaded into memory and grouped there |
| `items` | `[]` when expanded, `null` when collapsed. Child rows are **not** included; the client loads them with a follow-up request filtered on the group key | real rows / nested groups when expanded, `null` when collapsed |
| `summary` | never | per-group values from `groupSummary` |

Use the fast path for large tables. The full path scales with the filtered row count.
The grouped path does not call `HydratePage`. Grouping only requires the field to be declared in the
grid configuration; `sortable`/`filterable` flags are not checked. An undeclared group field yields empty `data`.

### Summaries

`summaryType`: `sum`, `avg`, `min`, `max` (numeric fields: `int`, `decimal`, `double`, `float`)
and `count` (any field). Anything else, such as `sum` on a string or `long`, or `min`/`max` on a date,
is skipped rather than failing. `totalSummary` is computed over the filtered set *before* paging.

### DevExtreme integration

Send DevExtreme's `loadOptions` object as-is under `loadOptions`. When present it is mapped onto the
request (overriding `skip`, `take`, `sort`, `filter`, search, `group`, and summaries):

```js
const store = new DevExpress.data.CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    const res = await fetch('/api/grids/tcnProfiles', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ loadOptions }),
    });
    const r = await res.json();
    return { data: r.data, totalCount: r.totalCount, groupCount: r.groupCount, summary: r.summary };
  },
});
```

(The frontend lives outside this repo; the snippet is illustrative.)

Field-name translation: for `loadOptions.sort`, `loadOptions.filter` and `searchExpr`, a selector is
matched to a configured field by exact key, then case-insensitive key, then `sourceMember`. So
`Id`, `id` and `ID` all work. **`group` and summary selectors are not translated**; they must equal
the configured field key exactly. In a plain (non-`loadOptions`) request, `sort.field`,
`filter` field names, and everything else must also match the key exactly (keys are case-sensitive).

If `loadOptions` can't be mapped, the error is written to the console and the request continues with
whatever was already set. A malformed `loadOptions.filter` is dropped (no filtering); a malformed
top-level `filter` is rejected during model binding.

## Available grids

### `tcnProfiles`

TCN profile listing (`TCNProfileGridRowDto` over `TcnProfile`; soft-deleted profiles are excluded by
the entity's global query filter). Default sort `createdAt desc`; max page size 1000;
`RequiredPermission = "tcnprofiles"`.

| Field | Type | Searchable |
|---|---|---|
| `id` | long | |
| `recamasId` | string | yes |
| `firstNameEl`, `lastNameEl`, `firstNameEn`, `lastNameEn` | string | yes |
| `gender` | `Male` / `Female` / `Unknown` | |
| `dateOfBirth` | date (nullable) | |
| `status` | `AVRApplicationPending`, `AVRReturnPending`, `Departed`, `ForcedReturnPreliminaryDetention`, `ForcedReturnPending`, `ByOwnReturnPending`, `Dismissed` | |
| `primarySource` | `ARS` / `CASS` / `Manual` / `Case` / `PoliceDb` | |
| `arc`, `mdFileNo`, `cassFileNo` | string | yes |
| `flagSecurityIssues`, `flagMinor`, `flagNoArc`, `flagNoTravelDocument` | bool | |
| `createdAt` | DateTimeOffset | |

All fields are sortable and filterable.

## Adding a new grid

No controller or routing changes are needed; once registered, the grid is live at
`POST /api/grids/{key}`. Follow the `TCNProfiles` files as a template.

**1. Row DTO** (`src/Application/Grids/Orders/OrderGridRowDto.cs`): a flat class with exactly the
columns the grid shows. Enums are serialised as strings.

```csharp
public sealed class OrderGridRowDto
{
    public long Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**2. Configuration** (`src/Application/Grids/Orders/OrderGridConfiguration.cs`): declare the fields
and their capabilities.

```csharp
public sealed class OrderGridConfiguration : GridConfiguration<OrderGridRowDto>
{
    private readonly GridConfigurationBuilder<OrderGridRowDto>.BuiltGridConfiguration _built;

    public OrderGridConfiguration()
    {
        _built = new GridConfigurationBuilder<OrderGridRowDto>()
            .Field("id",        typeof(long))
            .Field("number",    typeof(string), searchable: true)
            .Field("status",    typeof(OrderStatus))
            .Field("createdAt", typeof(DateTime))
            .DefaultSortBy("createdAt", desc: true)
            .MaxPageSize(500)
            .Build("orders");
    }

    public override string Key => _built.Key;
    public override IReadOnlyDictionary<string, GridField> Fields => _built.Fields;
    public override IReadOnlyList<SortDescriptor> DefaultSort => _built.DefaultSort;
    public override int MaxPageSize => _built.MaxPageSize;
}
```

`Field(name, dataType, searchable = false, sortable = true, filterable = true, sourceMember = null)`

- `name`: the public key clients send (camelCase by convention).
- `dataType`: the exact CLR type of the DTO property (including nullability, such as `DateOnly?`);
  it drives value coercion.
- `sourceMember`: the DTO property name when it isn't just the camelCase of `name`. Otherwise the
  name is matched to the DTO property ignoring first-letter case.

**3. Provider** (`src/Infrastructure/Grids/Orders/OrderGridSourceProvider.cs`): project entities to
the DTO.

```csharp
public sealed class OrderGridSourceProvider : GridSourceProvider<OrderGridRowDto>
{
    public override string? RequiredPermission => "orders";

    public OrderGridSourceProvider(OrderGridConfiguration configuration)
        : base(configuration, BuildQuery) { }

    private static IQueryable<OrderGridRowDto> BuildQuery(IApplicationDbContext db) =>
        from o in db.Set<Order>()
        select new OrderGridRowDto { Id = o.Id, Number = o.Number, Status = o.Status, CreatedAt = o.CreatedAt };
}
```

The query must be translatable by EF Core, because filter/sort/paging are composed on top of it. Don't
capture the `DbContext` in the provider: providers are singletons and the context is passed in per request.

**4. Register** in `InfrastructureServiceRegistration.cs`, under the data grid providers section:

```csharp
services.AddSingleton<OrderGridConfiguration>();
services.AddSingleton<IGridSourceProviderMarker, OrderGridSourceProvider>();
```
