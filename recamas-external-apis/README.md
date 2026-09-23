# CIT-RECAMAS — External Interface APIs

Τέσσερα .NET 9 Web APIs που προσομοιώνουν τα εξωτερικά συστήματα ολοκλήρωσης του CIT-RECAMAS,
βασισμένα στα §9.2–9.5 του Requirements Verification Document (Deliverable 1.2).

---

## Αρχιτεκτονική

```
┌───────────────────────────────────────────────────────────┐
│                   Docker Network                          │
│                                                           │
│  ┌──────────┐  ┌──────────┐  ┌───────────┐  ┌────────┐    │
│  │ ARS API  │  │ CASS API │  │ Arrivals  │  │Stoplist│    │
│  │ :5001    │  │ :5002    │  │ API :5003 │  │API:5004│    │
│  └────┬─────┘  └────┬─────┘  └─────┬─────┘  └───┬────┘    │
│       │              │               │              │     │
│       └──────────────┴───────────────┴──────────────┘     │
│                              │                            │
│                    ┌─────────▼──────────┐                 │
│                    │   PostgreSQL :5432  │                │
│                    │  recamas_interfaces │                │
│                    │                     │                │
│                    │  • ars_records      │                │
│                    │  • cass_records     │                │
│                    │  • arrival_records  │                │
│                    │  • stoplist_records │                │
│                    └─────────────────────┘                │
└───────────────────────────────────────────────────────────┘
```

Κάθε API:
- Ακολουθεί **Clean Architecture** (Domain → Application → Infrastructure → WebApi)
- Χρησιμοποιεί **Entity Framework Core 9** με PostgreSQL
- Κατά την 1η εκκίνηση δημιουργεί αυτόματα το schema και **φορτώνει 20 dummy records**
- Τα 5 πρώτα ARC (`ARC-001-SYR` έως `ARC-005-NGA`) είναι **κοινά** σε όλα τα APIs
- Παρέχει **Swagger UI** στο root URL κάθε API

---

## Προαπαιτούμενα

- **Docker Desktop** (ή Docker Engine + Docker Compose)
- Ports `5001`, `5002`, `5003`, `5004`, `5432` ελεύθερα στον υπολογιστή σου

---

## Εκκίνηση

```bash
# 1. Κλωνοποίησε / αποσυμπίεσε το solution
cd recamas-interfaces

# 2. Ξεκίνα όλα τα containers
docker compose up --build

# Ή στο background:
docker compose up --build -d
```

Κατά την πρώτη εκκίνηση:
1. Το Docker κατεβάζει το image του PostgreSQL 16
2. Χτίζει τα 4 .NET 9 images (περίπου 2-3 λεπτά)
3. Κάθε API περιμένει το PostgreSQL να είναι έτοιμο (retry loop)
4. Δημιουργεί τα tables αυτόματα (`EnsureCreated`)
5. Φορτώνει τα dummy data

---

## Swagger UIs

| API | URL | §Spec |
|-----|-----|-------|
| ARS (Alien Registration System) | http://localhost:5001 | §9.2 |
| CASS (Cyprus Asylum Service) | http://localhost:5002 | §9.3 |
| Arrivals / Departures | http://localhost:5003 | §9.4 |
| Stoplist (Entry-ban register) | http://localhost:5004 | §9.5 |
| Police contract gateway | http://localhost:5005 | Published Police OpenAPI paths |

## UAT-compatible external contracts and authentication

The mocks expose the paths consumed by `RECAMAS_INTEROPERABILITY_POC`:

- ARS: `GET /api/v1/Alien/immigration-applicants` and `/{docNumber}`; header `api-Key`.
- Police Stoplist: `POST /police/police-checks/v1/stoplist/search`.
- Police movements: `POST /police/police-checks/v1/arrivals-departures/search`.
- CASS: `POST /uat/v1/cass/search`, a provisional UAT-only HTTP contract pending the real WSDL.

Police and provisional CASS calls require `api-key`, `X-Client-ID`, and `X-Correlation-ID`.
Missing/invalid API keys return 401, unauthorized client IDs return 403, and missing required
correlation IDs return 400. ARS follows its supplied contract and requires only `api-Key`.

Development defaults can be overridden with `ARS_API_KEY`, `CASS_API_KEY`, `POLICE_API_KEY`, and
`UAT_CLIENT_ID`. Use overrides for every shared environment.

---

## Endpoints

Κάθε API έχει **2 endpoints**:

### `GET /api/{resource}/search`
Αναζήτηση βάσει κριτηρίων. Τουλάχιστον **ένα** παράμετρο απαιτείται.

**Παράδειγμα — ARS:**
```bash
# Αναζήτηση με ARC
curl "http://localhost:5001/api/ars/search?arc=ARC-001-SYR"

# Αναζήτηση με επίθετο
curl "http://localhost:5001/api/ars/search?surname=khalil"

# Αναζήτηση με εθνικότητα (enum ως string)
curl "http://localhost:5001/api/ars/search?nationality=Syrian"

# CASS — ίδια λογική
curl "http://localhost:5002/api/cass/search?arc=ARC-003-IRQ"

# Arrivals — εμφανίζει όλες τις αφίξεις/αναχωρήσεις TCN
curl "http://localhost:5003/api/arrivals/search?arc=ARC-003-IRQ"

# Stoplist — έλεγχος entry ban
curl "http://localhost:5004/api/stoplist/search?arc=ARC-003-IRQ"
```

### `POST /api/{resource}`
Δημιουργία νέας εγγραφής.

**Παράδειγμα — Stoplist:**
```bash
curl -X POST "http://localhost:5004/api/stoplist" \
  -H "Content-Type: application/json" \
  -d '{
    "arc": "ARC-NEW-001",
    "firstName": "John",
    "lastName": "Doe",
    "nationality": "Syrian",
    "passportNo": "P-SYR-NEW",
    "dateOfBirth": "1990-01-15",
    "isOnStoplist": true,
    "uniqueEntryBanNumber": "BAN-CY-2025-999",
    "stoplistEntryDate": "2025-07-01"
  }'
```

---

## Κοινά ARCs (υπάρχουν σε όλα τα APIs)

| ARC | Όνομα | Εθνικότητα | Σχόλιο |
|-----|-------|------------|--------|
| ARC-001-SYR | Ahmad Al-Hassan | Syrian | Voluntary return ordered |
| ARC-002-AFG | Farida Ahmadi | Afghan | IP application pending |
| ARC-003-IRQ | Omar Khalil | Iraqi | Forced return + Entry ban 60 mths |
| ARC-004-PAK | Aisha Malik | Pakistani | Active resident |
| ARC-005-NGA | Emmanuel Okafor | Nigerian | AVR program approved |

---

## Δομή Solution (Clean Architecture)

```
recamas-interfaces/
├── docker-compose.yml
├── RecamasInterfaces.sln
└── src/
    ├── Shared/
    │   └── Domain/                    ← Κοινά enums (Nationality, Gender, Airport...)
    │
    ├── ArsApi/
    │   ├── Domain/Entities/           ← ArsRecord (τα πεδία του spec)
    │   ├── Application/
    │   │   ├── DTOs/                  ← Request/Response records
    │   │   ├── Interfaces/            ← IArsRepository
    │   │   └── Services/              ← ArsService (business logic)
    │   ├── Infrastructure/
    │   │   ├── Data/                  ← ArsDbContext + ArsSeeder
    │   │   └── Repositories/          ← ArsRepository (EF Core queries)
    │   └── WebApi/
    │       ├── Controllers/           ← ArsController (2 endpoints)
    │       ├── Program.cs
    │       ├── appsettings.json
    │       └── Dockerfile
    │
    ├── CassApi/    (same structure)
    ├── ArrivalsApi/ (same structure)
    └── StoplistApi/ (same structure)
```

---

## Διακοπή / Καθαρισμός

```bash
# Σταμάτα τα containers
docker compose down

# Σταμάτα ΚΑΙ διέγραψε τη βάση δεδομένων
docker compose down -v

# Rebuild μετά από αλλαγές στον κώδικα
docker compose up --build
```

---

## Σύνδεση στη βάση (προαιρετικά)

```
Host:     localhost
Port:     5432
Database: recamas_interfaces
Username: recamas
Password: recamas_pass
```

```sql
-- Προβολή tables
\dt

-- Έλεγχος records
SELECT arc, first_name, last_name, nationality FROM ars_records;
SELECT arc, ip_status_type, ip_status_decision FROM cass_records;
SELECT arc, movement_type, movement_date FROM arrival_records;
SELECT arc, is_on_stoplist, unique_entry_ban_number FROM stoplist_records;

-- Cross-check: κοινά ARCs
SELECT a.arc, a.nationality as ars_nat, c.cass_file_no, s.is_on_stoplist
FROM ars_records a
JOIN cass_records c ON a.arc = c.arc
JOIN stoplist_records s ON a.arc = s.arc
ORDER BY a.arc;
```
