using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace RECAMAS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cases");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.EnsureSchema(
                name: "tcn_profile");

            migrationBuilder.EnsureSchema(
                name: "detention");

            migrationBuilder.EnsureSchema(
                name: "return_implementation");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,")
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "app_settings",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SettingKey = table.Column<string>(type: "text", nullable: false, comment: "The key of the setting or parameter"),
                    SettingValue = table.Column<string>(type: "text", nullable: false, comment: "The value of the setting — string"),
                    DisplayName = table.Column<string>(type: "text", nullable: true, comment: "Το όνομα της παραμέτρου όπως αυτή θα εμφανίζεται στο UI"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Σύντομο κείμενο που να εξηγεί την παράμετρο ή την ρύθμιση"),
                    Data_Type = table.Column<string>(type: "text", nullable: true, comment: "Ο τύπος της τιμής SettingValue"),
                    Category = table.Column<string>(type: "text", nullable: true, comment: "Ο τύπος της τιμής SettingValue"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsEncrypted = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "audit_outbox",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Action = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Attempts = table.Column<int>(type: "integer", nullable: false),
                    NextAttemptAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_outbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "business_rules",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleId = table.Column<string>(type: "text", nullable: false, comment: "Human-readable system ID — string"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WorkflowType = table.Column<int>(type: "integer", nullable: false),
                    Programme = table.Column<string>(type: "text", nullable: true, comment: "Programme code ή null (= AllProgrammes) — string: codelist value"),
                    FromStage = table.Column<int>(type: "integer", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStage = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    ConditionExpression = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON DSL — string (structured expression tree)"),
                    ThenAction = table.Column<int>(type: "integer", nullable: false),
                    TargetFieldOrDocumentType = table.Column<string>(type: "text", nullable: true, comment: "Field name ή doc type — string (dynamic)"),
                    UserFacingMessage = table.Column<string>(type: "text", nullable: false, comment: "Μήνυμα — free text"),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ValidFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RuleStatus = table.Column<int>(type: "integer", nullable: false, comment: "Draft | Scheduled | Effective | Expired"),
                    ClonedFromRuleId = table.Column<long>(type: "bigint", nullable: true),
                    IsClone = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_business_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_business_rules_business_rules_ClonedFromRuleId",
                        column: x => x.ClonedFromRuleId,
                        principalSchema: "admin",
                        principalTable: "business_rules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "codelists",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ListCode = table.Column<string>(type: "text", nullable: false, comment: "Μοναδικός κωδικός — string (πχ. \"COUNTRY\")"),
                    DisplayName = table.Column<string>(type: "text", nullable: false, comment: "String"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_codelists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "detention_centers",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Επωνυμία — free text"),
                    Location = table.Column<string>(type: "text", nullable: true, comment: "Τοποθεσία — free text"),
                    TotalCapacity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_centers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "document_templates",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CaseType = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<int>(type: "integer", nullable: true, comment: "Greek | English | Other"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    TemplateFilePath = table.Column<string>(type: "text", nullable: false, comment: "Storage path"),
                    PlaceholdersDefinition = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON — string (structured data)"),
                    PreviousVersionId = table.Column<long>(type: "bigint", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_document_templates_document_templates_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalSchema: "admin",
                        principalTable: "document_templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "interface_sync_logs",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalSystem = table.Column<int>(type: "integer", nullable: false),
                    TriggerType = table.Column<int>(type: "integer", nullable: false),
                    TriggeredByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedTcnProfileId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedCaseId = table.Column<long>(type: "bigint", nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true, comment: "Free text (technical message)"),
                    RetryCount = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interface_sync_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipientUserId = table.Column<long>(type: "bigint", nullable: false),
                    TriggeredByUserId = table.Column<long>(type: "bigint", nullable: true),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    Channel = table.Column<int>(type: "integer", nullable: false, comment: "InApp | Email | Both"),
                    Title = table.Column<string>(type: "text", nullable: false, comment: "Τίτλος — free text (template-generated)"),
                    Body = table.Column<string>(type: "text", nullable: true, comment: "Σώμα — free text"),
                    DeepLinkEntityType = table.Column<int>(type: "integer", nullable: true, comment: "Case | Implementation | Profile"),
                    DeepLinkEntityId = table.Column<long>(type: "bigint", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EmailSent = table.Column<bool>(type: "boolean", nullable: false),
                    EmailSentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "return_implementations",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImplementationId = table.Column<string>(type: "text", nullable: false, comment: "System-generated — string"),
                    ImplementationType = table.Column<int>(type: "integer", nullable: false),
                    InitiationDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PlannedExecutionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReturnCountryCode = table.Column<string>(type: "text", nullable: false, comment: "ISO 3166-1 alpha-2 — string (standard country code)"),
                    TransitCountryCodes = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON array of ISO codes — string[] serialized"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OperationOwner = table.Column<int>(type: "integer", nullable: true, comment: "FRONTEX | CyprusAuthorities | Other"),
                    FlightNumber = table.Column<string>(type: "text", nullable: true, comment: "Free text (airline format)"),
                    CancellationReason = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_implementations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tcn_profiles",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecamasId = table.Column<string>(type: "text", nullable: false, comment: "Μοναδικό system-generated ID (πχ. TCN-2026-00001)"),
                    Arc = table.Column<string>(type: "text", nullable: true, comment: "Alien Registration Card number — από ARS interface"),
                    EurodacNumber = table.Column<string>(type: "text", nullable: true, comment: "EURODAC biometric reference — από CASS"),
                    FirstNameEl = table.Column<string>(type: "text", nullable: true, comment: "Όνομα στα Ελληνικά — από ARS/Manual"),
                    FirstNameEn = table.Column<string>(type: "text", nullable: true, comment: "Όνομα στα Αγγλικά — από ARS/Manual"),
                    MiddleNameEl = table.Column<string>(type: "text", nullable: true, comment: "Μεσαίο όνομα EL"),
                    MiddleNameEn = table.Column<string>(type: "text", nullable: true, comment: "Μεσαίο όνομα EN"),
                    LastNameEl = table.Column<string>(type: "text", nullable: true, comment: "Επώνυμο EL"),
                    LastNameEn = table.Column<string>(type: "text", nullable: true, comment: "Επώνυμο EN"),
                    Gender = table.Column<int>(type: "integer", nullable: false, comment: "M / F / U (Unknown) — από ARS"),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true, comment: "Ημερομηνία γέννησης — από ARS/Manual"),
                    PlaceOfBirth = table.Column<string>(type: "text", nullable: true, comment: "Τόπος γέννησης — free text (πόλη ή περιοχή, όχι enum: αχανές set)"),
                    MdFileNo = table.Column<string>(type: "text", nullable: true, comment: "Migration Department file number — από ARS"),
                    ArsFolderId = table.Column<string>(type: "text", nullable: true, comment: "[ΠΡΟΣΤΕΘΗΚΕ] Βάση για πρόταση linked-profile — έλειπε, βλ. tcn_profile.ars_folder_id PDF ref: §2.2.4 \"Linked Profile Suggestion\" — \"Based on the ARS Folder Number, RECAMAS performs an interface call to ARS and retrieves any TCN Profiles that have the same ARS Folder Number\""),
                    MdFileRelationship = table.Column<int>(type: "integer", nullable: true, comment: "Ρόλος στο MD file: Principal / MainDependant / Dependant"),
                    CassFileNo = table.Column<string>(type: "text", nullable: true, comment: "Cyprus Asylum Service file number — από CASS"),
                    CassAddress = table.Column<string>(type: "text", nullable: true, comment: "Διεύθυνση από CASS (read-only, free text)"),
                    MdAddress = table.Column<string>(type: "text", nullable: true, comment: "Διεύθυνση από ARS (read-only, free text)"),
                    CassPhone = table.Column<string>(type: "text", nullable: true, comment: "Τηλέφωνο από CASS (read-only)"),
                    MdPhone = table.Column<string>(type: "text", nullable: true, comment: "Τηλέφωνο από ARS (read-only)"),
                    PhotographStoragePath = table.Column<string>(type: "text", nullable: true, comment: "Path στο storage (ISO/IEC 19794-5:2011)"),
                    FingerprintNistPath = table.Column<string>(type: "text", nullable: true, comment: "Path NIST ITL 1-2011 file"),
                    FingerprintVector = table.Column<Vector>(type: "vector", nullable: true, comment: "pgvector embedding για biometric search"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "§3.4: AVRApplicationPending, AVRReturnPending, Departed, κλπ"),
                    FlagSecurityIssues = table.Column<bool>(type: "boolean", nullable: false, comment: "§3.5: Υπάρχει τουλάχιστον ένα security issue item"),
                    FlagMinor = table.Column<bool>(type: "boolean", nullable: false, comment: "§3.5: Ηλικία < 18"),
                    FlagNoArc = table.Column<bool>(type: "boolean", nullable: false, comment: "§3.5: Δεν υπάρχει ARC"),
                    FlagNoTravelDocument = table.Column<bool>(type: "boolean", nullable: false, comment: "§3.5: Δεν υπάρχει travel document για επιστροφή"),
                    IsAnonymized = table.Column<bool>(type: "boolean", nullable: false, comment: "Έχει γίνει anonymization βάσει retention policy"),
                    AnonymizedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Πότε έγινε anonymization"),
                    AnonymizedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "Ποιος έκανε anonymization"),
                    PrimarySource = table.Column<int>(type: "integer", nullable: false, comment: "ARS | CASS | Manual — από που δημιουργήθηκε το profile"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "codelist_values",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodelistId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Μοναδικός κωδικός τιμής — string"),
                    DisplayNameEl = table.Column<string>(type: "text", nullable: false),
                    DisplayNameEn = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_codelist_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_codelist_values_codelists_CodelistId",
                        column: x => x.CodelistId,
                        principalSchema: "admin",
                        principalTable: "codelists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_wings",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetentionCenterId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Όνομα πτέρυγας — free text"),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_wings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_wings_detention_centers_DetentionCenterId",
                        column: x => x.DetentionCenterId,
                        principalSchema: "detention",
                        principalTable: "detention_centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "implementation_documents",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnImplementationId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UploadedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    UploadDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AttachmentPath = table.Column<string>(type: "text", nullable: false, comment: "UUID filename"),
                    MimeType = table.Column<string>(type: "text", nullable: true, comment: "IANA MIME type — string (standard)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_implementation_documents_return_implementations_ReturnImple~",
                        column: x => x.ReturnImplementationId,
                        principalSchema: "return_implementation",
                        principalTable: "return_implementations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "implementation_escort_team_members",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnImplementationId = table.Column<long>(type: "bigint", nullable: false),
                    EscortType = table.Column<int>(type: "integer", nullable: false),
                    EscortUserId = table.Column<long>(type: "bigint", nullable: true, comment: "Αναγνωριστικό χρήστη από το εξωτερικό identity system"),
                    ExternalEscortName = table.Column<string>(type: "text", nullable: true, comment: "Free text (αν εξωτερικός)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_escort_team_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_implementation_escort_team_members_return_implementations_R~",
                        column: x => x.ReturnImplementationId,
                        principalSchema: "return_implementation",
                        principalTable: "return_implementations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "implementation_other_expenses",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnImplementationId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false),
                    ApprovedExpenseAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Από Case"),
                    ActualExpenseAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true, comment: "Εισάγεται κατά implementation"),
                    AttachmentsPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_other_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_implementation_other_expenses_return_implementations_Return~",
                        column: x => x.ReturnImplementationId,
                        principalSchema: "return_implementation",
                        principalTable: "return_implementations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "return_cases",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<string>(type: "text", nullable: false, comment: "Human-readable system-generated ID (πχ. AVR-2026-001)"),
                    CaseType = table.Column<int>(type: "integer", nullable: false, comment: "AVR | ForcedReturn | ByOwn"),
                    Stage = table.Column<int>(type: "integer", nullable: false, comment: "Counselling | ApplicationProcessing | Detention | κλπ"),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Initiated | OnHold | PendingApproval | κλπ"),
                    InitiationDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "System-generated κατά δημιουργία"),
                    InitiationOffice = table.Column<int>(type: "integer", nullable: true, comment: "MD | συγκεκριμένο A&IU office"),
                    ImplementationOffice = table.Column<string>(type: "text", nullable: true, comment: "A&IU office υλοποίησης — free text (codelist-driven από admin)"),
                    ReturnCountryCode = table.Column<string>(type: "text", nullable: true, comment: "ISO 3166-1 alpha-2 — string (standard country code)"),
                    ReturnReason = table.Column<int>(type: "integer", nullable: true, comment: "ILMigrant | AsylumSeeker | AsylumRejection | κλπ"),
                    InternationalFramework = table.Column<int>(type: "integer", nullable: true, comment: "EUReadmission | Bilateral | κλπ"),
                    FlagNoArc = table.Column<bool>(type: "boolean", nullable: false),
                    FlagNoTravelDocument = table.Column<bool>(type: "boolean", nullable: false),
                    FlagMinor = table.Column<bool>(type: "boolean", nullable: false),
                    FlagUnaccompaniedMinor = table.Column<bool>(type: "boolean", nullable: false),
                    FlagCriminalRecord = table.Column<bool>(type: "boolean", nullable: false),
                    FlagRestrictiveActivities = table.Column<bool>(type: "boolean", nullable: false),
                    FlagHealthIssues = table.Column<bool>(type: "boolean", nullable: false),
                    FlagNeedsAttention = table.Column<bool>(type: "boolean", nullable: false, comment: "Interface update σε TCN profile (auto On Hold)"),
                    FlagException = table.Column<bool>(type: "boolean", nullable: false, comment: "Χώρα εκτός προγράμματος"),
                    FlagProgramSwitch = table.Column<bool>(type: "boolean", nullable: false, comment: "AVR Cyprus ↔ EURP switch"),
                    IsOnHold = table.Column<bool>(type: "boolean", nullable: false),
                    OnHoldReason = table.Column<int>(type: "integer", nullable: true, comment: "ManuallyPlaced | InterfaceUpdate"),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    CancellationReason = table.Column<string>(type: "text", nullable: true, comment: "Ελεύθερο κείμενο — απαιτείται (§4.3.2)"),
                    CancelledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReturnImplementationId = table.Column<long>(type: "bigint", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_return_cases_return_implementations_ReturnImplementationId",
                        column: x => x.ReturnImplementationId,
                        principalSchema: "return_implementation",
                        principalTable: "return_implementations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "appeals",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfAppeal = table.Column<int>(type: "integer", nullable: false, comment: "AdministrativeCourt | Other"),
                    AppealNumber = table.Column<string>(type: "text", nullable: true, comment: "Μοναδικός αριθμός έφεσης — free text (αριθμός πρωτοκόλλου)"),
                    AppealDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AppealStatusDecision = table.Column<int>(type: "integer", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appeals_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "arrival_departures",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    MovementType = table.Column<int>(type: "integer", nullable: false, comment: "Arrival | Departure"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    AirportCode = table.Column<string>(type: "text", nullable: true, comment: "IATA airport code — string (standard code, πχ. \"LCA\")"),
                    LastSyncedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arrival_departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_arrival_departures_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_documents",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false, comment: "ARS | CASS | RECAMAS | Case"),
                    DocumentType = table.Column<int>(type: "integer", nullable: false, comment: "Passport | CountryIssuedId | Other"),
                    IsTravelDocument = table.Column<bool>(type: "boolean", nullable: false, comment: "True → μπορεί να χρησιμοποιηθεί σε Return Case"),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true, comment: "Αριθμός εγγράφου — free text (διαφορετική μορφή ανά χώρα)"),
                    IssuingCountryCode = table.Column<string>(type: "text", nullable: true, comment: "ISO 3166-1 alpha-2 — string (standard country code)"),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true, comment: "Αρχή έκδοσης — free text (ανοιχτό set ανά χώρα)"),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true, comment: "Storage path του scan"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_identity_documents_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ip_applications",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfApplication = table.Column<int>(type: "integer", nullable: false),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StatusDecision = table.Column<int>(type: "integer", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ip_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ip_applications_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ip_statuses",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfStatus = table.Column<int>(type: "integer", nullable: false, comment: "RefugeeStatus | SubsidiaryProtection | Other"),
                    DateOfGranting = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StatusDecision = table.Column<int>(type: "integer", nullable: false, comment: "Pending | Approved | Rejected | Revoked | Other"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ip_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ip_statuses_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "linked_profiles",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromTcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ToTcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Relationship = table.Column<int>(type: "integer", nullable: false, comment: "Spouse | Child | Parent | Sibling | DuplicateMerged | Other"),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Σημειώσεις — free text"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_linked_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_linked_profiles_tcn_profiles_FromTcnProfileId",
                        column: x => x.FromTcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_linked_profiles_tcn_profiles_ToTcnProfileId",
                        column: x => x.ToTcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "residency_applications",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfPermitRequested = table.Column<int>(type: "integer", nullable: true, comment: "Τύπος αιτούμενης άδειας"),
                    TypeOfApplication = table.Column<int>(type: "integer", nullable: true, comment: "Initial | Renewal | Replacement"),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResidenceCategory = table.Column<int>(type: "integer", nullable: true),
                    PurposeRnd = table.Column<string>(type: "text", nullable: true, comment: "RND code — free text, ARS-specific"),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Pending | Approved | Rejected"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_residency_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_residency_applications_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "residency_statuses",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PermitType = table.Column<int>(type: "integer", nullable: true, comment: "Τύπος άδειας παραμονής / ταξιδίου"),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResidenceCategory = table.Column<int>(type: "integer", nullable: true, comment: "Κατηγορία παραμονής"),
                    PurposeRnd = table.Column<string>(type: "text", nullable: true, comment: "Κωδικός / περιγραφή σκοπού παραμονής (RND) — free text, ARS-specific"),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Active | Expired | Revoked"),
                    ResidencyDocumentNumber = table.Column<string>(type: "text", nullable: true, comment: "Μοναδικός αριθμός άδειας — free text"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_residency_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_residency_statuses_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "security_details",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    NoCriminalRecordFound = table.Column<bool>(type: "boolean", nullable: false),
                    NoRestrictiveActivitiesFound = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_security_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_security_details_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stoplist_entries",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    StoplistHit = table.Column<bool>(type: "boolean", nullable: false, comment: "Βρέθηκε ή όχι στη stoplist"),
                    StoplistReason = table.Column<string>(type: "text", nullable: true, comment: "Λόγος εγγραφής — free text (από Police DB)"),
                    UniqueEntryBanNumber = table.Column<string>(type: "text", nullable: true, comment: "Μοναδικός αριθμός απαγόρευσης — free text (Police DB format)"),
                    StoplistEntryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true, comment: "Από Case"),
                    EntryBanExpirationDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Υπολογίζεται από Implementation"),
                    LastSyncedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Τελευταίος συγχρονισμός από Police DB"),
                    Source = table.Column<int>(type: "integer", nullable: false, comment: "[ΠΡΟΣΤΕΘΗΚΕ] default POLICE_DB — έλειπε, βλ. tcn_stoplist_entry.source PDF ref: §9.5 \"Stoplist\" — Stoplist is a system maintained in the Police Database (§9.5, §9.5.1 Interface execution)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stoplist_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stoplist_entries_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_nationalities",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false, comment: "FK → TcnProfile"),
                    CountryCode = table.Column<string>(type: "text", nullable: false, comment: "ISO 3166-1 alpha-2 — string γιατί είναι standard κωδικός (πχ. \"CY\", \"GR\")"),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, comment: "Κύρια εθνικότητα για reporting"),
                    IdentificationStatus = table.Column<int>(type: "integer", nullable: false, comment: "Confirmed | Claimed | Unknown"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_nationalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_nationalities_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_rooms",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetentionWingId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Αναγνωριστικό δωματίου — free text"),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_rooms_detention_wings_DetentionWingId",
                        column: x => x.DetentionWingId,
                        principalSchema: "detention",
                        principalTable: "detention_wings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "implementation_escort_expenses",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImplementationEscortTeamMemberId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false),
                    ExpenseAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_escort_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_implementation_escort_expenses_implementation_escort_team_m~",
                        column: x => x.ImplementationEscortTeamMemberId,
                        principalSchema: "return_implementation",
                        principalTable: "implementation_escort_team_members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "additional_approvers",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    ApproverUserId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    OrderInChain = table.Column<int>(type: "integer", nullable: false, comment: "Σειρά στο chain"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additional_approvers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_additional_approvers_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "adjustment_notes",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false, comment: "Λόγος — free text"),
                    SubmittedForAdjustmentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adjustment_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adjustment_notes_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    AssessmentDecision = table.Column<int>(type: "integer", nullable: false, comment: "Pending | Approved | Rejected"),
                    AssessmentDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssessedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApproverNotes = table.Column<string>(type: "text", nullable: true, comment: "Σημειώσεις εγκριτή — free text"),
                    RevocationDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevocationReason = table.Column<string>(type: "text", nullable: true, comment: "Λόγος ανάκλησης — free text"),
                    RelatedIssueId = table.Column<long>(type: "bigint", nullable: true, comment: "FK → DetentionUpdateIssue"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_approval_items_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "avr_cases",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    Program = table.Column<int>(type: "integer", nullable: false, comment: "AVRCyprus | EURP"),
                    CounsellingSessionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avr_cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_avr_cases_return_cases_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "by_own_cases",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_by_own_cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_by_own_cases_return_cases_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_assignments",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UnassignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UnassignedReason = table.Column<int>(type: "integer", nullable: true, comment: "Submitted | ManualUnassign"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_assignments_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_documents",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: true, comment: "Null αν αφορά το case συνολικά"),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false, comment: "Generated | Uploaded"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    UploadedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UploadDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AttachmentPath = table.Column<string>(type: "text", nullable: false, comment: "Storage path — UUID filename (§12.5.16)"),
                    OriginalFilename = table.Column<string>(type: "text", nullable: true, comment: "Αρχικό όνομα πριν rename"),
                    MimeType = table.Column<string>(type: "text", nullable: true, comment: "MIME type για validation — string (standard IANA)"),
                    IsQesSigned = table.Column<bool>(type: "boolean", nullable: false),
                    QesVerified = table.Column<bool>(type: "boolean", nullable: true),
                    QesSignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    QesSignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    JccTransactionId = table.Column<string>(type: "text", nullable: true, comment: "JCC reference — free text (external system format)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_documents_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_documents_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "case_history_entries",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    EntryType = table.Column<int>(type: "integer", nullable: false, comment: "EntryCreation | DataUpdate | Assignment | Action | StageStatusChange | FlagChange"),
                    ActorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ActorRoleId = table.Column<long>(type: "bigint", nullable: true),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: true, comment: "Όνομα πεδίου — string (reflection-based)"),
                    PreviousValue = table.Column<string>(type: "text", nullable: true, comment: "JSON serialized — string"),
                    UpdatedValue = table.Column<string>(type: "text", nullable: true, comment: "JSON serialized — string"),
                    PreviousAssignment = table.Column<string>(type: "text", nullable: true, comment: "\"UserName (RoleName)\" — string"),
                    UpdatedAssignment = table.Column<string>(type: "text", nullable: true),
                    ActionDescription = table.Column<string>(type: "text", nullable: true, comment: "Περιγραφή — free text"),
                    EntryTypeName = table.Column<string>(type: "text", nullable: true, comment: "Τύπος εγγραφής — free text (πχ. \"TravelDocument\")"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_history_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_history_entries_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_requests",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<string>(type: "text", nullable: false, comment: "System-generated human-readable ID — string"),
                    RequestType = table.Column<int>(type: "integer", nullable: false, comment: "ExpediteProcess | DocumentRequest | InformationRequest"),
                    RequestedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    RecipientType = table.Column<int>(type: "integer", nullable: false, comment: "Role | User"),
                    RequestedToUserId = table.Column<long>(type: "bigint", nullable: true),
                    RequestedToRoleId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Requested | Answered | Closed"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_requests_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_tcns",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    NoCriminalRecordFound = table.Column<bool>(type: "boolean", nullable: false),
                    NoRestrictiveActivitiesFound = table.Column<bool>(type: "boolean", nullable: false),
                    FitToFly = table.Column<bool>(type: "boolean", nullable: false),
                    FitToFlyAttachmentPath = table.Column<string>(type: "text", nullable: true, comment: "Storage path"),
                    PreReturnTravelDocExists = table.Column<bool>(type: "boolean", nullable: false),
                    PreReturnTravelDocDelivered = table.Column<bool>(type: "boolean", nullable: false),
                    PreReturnTravelDocReceived = table.Column<bool>(type: "boolean", nullable: false),
                    PreReturnFitToFly = table.Column<bool>(type: "boolean", nullable: false),
                    PreReturnActivitiesCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    PreReturnNoOpenIssues = table.Column<bool>(type: "boolean", nullable: false, comment: "Μόνο FRC"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_tcns_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "forced_return_cases",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    Program = table.Column<int>(type: "integer", nullable: false, comment: "EURP | Cyprus"),
                    ApprehensionOfficer = table.Column<string>(type: "text", nullable: true, comment: "Ονοματεπώνυμο — free text"),
                    ApprehensionLocation = table.Column<string>(type: "text", nullable: true, comment: "Τοποθεσία — free text (ανοιχτό)"),
                    ApprehensionDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ApprehensionJustification = table.Column<string>(type: "text", nullable: true, comment: "Αιτιολόγηση — free text"),
                    PreliminaryDetentionLocation = table.Column<string>(type: "text", nullable: true, comment: "Τοποθεσία — free text"),
                    EscapeRisk = table.Column<int>(type: "integer", nullable: false, comment: "Low | Medium | High"),
                    AiuOfficerNotes = table.Column<string>(type: "text", nullable: true, comment: "Σημειώσεις — free text"),
                    SuggestionMemoPath = table.Column<string>(type: "text", nullable: true, comment: "Storage path")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forced_return_cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_forced_return_cases_return_cases_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "implementation_tcns",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnImplementationId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    TravelAgency = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    TicketCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Airline = table.Column<string>(type: "text", nullable: true, comment: "Free text (airline name)"),
                    FlightNumber = table.Column<string>(type: "text", nullable: true, comment: "Free text (airline format)"),
                    DepartureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DepartureAirportCode = table.Column<string>(type: "text", nullable: true, comment: "IATA code — string (standard)"),
                    DestinationCountryCode = table.Column<string>(type: "text", nullable: true, comment: "ISO 3166-1 alpha-2 — string"),
                    DestinationAirportCode = table.Column<string>(type: "text", nullable: true, comment: "IATA code — string"),
                    TicketNumber = table.Column<string>(type: "text", nullable: true, comment: "Free text (airline format)"),
                    TicketFilePath = table.Column<string>(type: "text", nullable: true),
                    InvoiceFilePath = table.Column<string>(type: "text", nullable: true),
                    TravelDocumentReceipt = table.Column<bool>(type: "boolean", nullable: false),
                    ReturnDecisionAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    SignedReturnDecisionPath = table.Column<string>(type: "text", nullable: true),
                    TicketProvided = table.Column<bool>(type: "boolean", nullable: false),
                    MonetaryIncentiveReceived = table.Column<bool>(type: "boolean", nullable: false),
                    MonetaryIncentiveReceiptPath = table.Column<string>(type: "text", nullable: true),
                    DepartureDetailsAttachmentsPath = table.Column<string>(type: "text", nullable: true),
                    Departed = table.Column<bool>(type: "boolean", nullable: false),
                    DepartureConfirmationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true),
                    EntryBanExpirationDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Calculated"),
                    StoplistConfirmation = table.Column<bool>(type: "boolean", nullable: false),
                    UniqueEntryBanNumber = table.Column<string>(type: "text", nullable: true, comment: "Από Stoplist — free text (Police DB format)"),
                    MonetaryIncentiveApprovedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonetaryIncentiveProvided = table.Column<bool>(type: "boolean", nullable: true),
                    MonetaryIncentiveAmountProvided = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonetaryIncentiveNotes = table.Column<string>(type: "text", nullable: true),
                    MonetaryIncentiveAttachmentPath = table.Column<string>(type: "text", nullable: true),
                    PostArrivalMonetaryIncentiveProvided = table.Column<bool>(type: "boolean", nullable: false),
                    AssistanceDeadline = table.Column<DateOnly>(type: "date", nullable: true, comment: "Calculated: DepartureDate + 5 months"),
                    PostArrivalEligible = table.Column<bool>(type: "boolean", nullable: false),
                    ByOwnReturnDecisionAccepted = table.Column<bool>(type: "boolean", nullable: true),
                    ByOwnSignedReturnDecisionPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_implementation_tcns_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_implementation_tcns_return_implementations_ReturnImplementa~",
                        column: x => x.ReturnImplementationId,
                        principalSchema: "return_implementation",
                        principalTable: "return_implementations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_implementation_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pre_return_checklists",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false, comment: "1:1 με ReturnCase"),
                    SignedOff = table.Column<bool>(type: "boolean", nullable: false),
                    SignedOffByUserId = table.Column<long>(type: "bigint", nullable: true),
                    SignedOffAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pre_return_checklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pre_return_checklists_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "return_decisions",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    IssuingAuthority = table.Column<int>(type: "integer", nullable: false, comment: "ARS | CASS | MD | CAS | Other"),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionText = table.Column<string>(type: "text", nullable: true, comment: "Κείμενο απόφασης — free text"),
                    TcnReceiptDate = table.Column<DateOnly>(type: "date", nullable: true),
                    VoluntaryReturnDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true),
                    DecisionFilePath = table.Column<string>(type: "text", nullable: true, comment: "Storage path"),
                    IssuingCaseId = table.Column<long>(type: "bigint", nullable: true, comment: "FK → ReturnCase (αν εκδόθηκε μέσω Case)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_return_decisions_return_cases_IssuingCaseId",
                        column: x => x.IssuingCaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_return_decisions_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "travel_document_issuances",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    IssuanceId = table.Column<string>(type: "text", nullable: false, comment: "System-generated — string"),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    RequestedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    RequestingAuthority = table.Column<int>(type: "integer", nullable: true, comment: "MigrationDepartment | AIU | Other"),
                    IssuingCountryCode = table.Column<string>(type: "text", nullable: true, comment: "ISO 3166-1 alpha-2 — string"),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true, comment: "Αρχή έκδοσης — free text (ανά χώρα διαφέρει)"),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Requested | Issued | Rejected"),
                    IssuedDocumentNumber = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    IssuedDocumentExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IssuedDocumentAttachmentPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_travel_document_issuances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_travel_document_issuances_return_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "cases",
                        principalTable: "return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_travel_document_issuances_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alternative_measure_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    MeasureType = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alternative_measure_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alternative_measure_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entry_ban_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_ban_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entry_ban_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "escort_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    EscortType = table.Column<int>(type: "integer", nullable: false, comment: "AIUOfficer | MedicalProfessional | Other"),
                    EscortNumber = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escort_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_escort_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "frc_order_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    OrderType = table.Column<int>(type: "integer", nullable: false, comment: "DetentionOrder | DeportationOrder"),
                    DocumentLanguage = table.Column<int>(type: "integer", nullable: true),
                    LegalBasis = table.Column<string>(type: "text", nullable: true, comment: "Νομική βάση — string: codelist τιμή ή free text (§4.5.2.4.1: \"list of values OR free text\")"),
                    OrderFilePath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frc_order_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_frc_order_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monetary_incentive_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "Pre-filled από Program/Country — editable"),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monetary_incentive_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monetary_incentive_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "other_expense_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false, comment: "Accommodation | Translation | Medical | Administrative | Other"),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_other_expense_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_other_expense_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "return_decision_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    DecisionId = table.Column<string>(type: "text", nullable: true, comment: "System-generated — string"),
                    DocumentName = table.Column<string>(type: "text", nullable: true, comment: "Τίτλος εγγράφου — free text"),
                    DocumentLanguage = table.Column<int>(type: "integer", nullable: true, comment: "Greek | English | Other"),
                    DecisionFilePath = table.Column<string>(type: "text", nullable: true),
                    PreparedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "[ΠΡΟΣΤΕΘΗΚΕ] Ξεχωριστό από ApprovalItem.AssessedByUserId (=approved by) — έλειπε η διάκριση \"συνέταξε\" vs \"ενέκρινε\", βλ. case_return_decision.prepared_by PDF ref: ΟΧΙ άμεσο match — το Table 40 \"AVR Return Decisions\" (§4.4.2.4.1) έχει μόνο \"Approved by\". Αναλογία με Table 91 \"BOR Return Decision Issuance\" (§4.6.2.3), που έχει ρητό πεδίο \"Prepared by\" στην κλάση ByOwnReturnDecisionIssuance — χρειάζεται επιβεβαίωση αν ισχύει και εδώ"),
                    DecisionDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VoluntaryDepartureDeadline = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_decision_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_return_decision_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "travel_doc_issuance_approval_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity"),
                    TravelDocumentType = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_travel_doc_issuance_approval_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_travel_doc_issuance_approval_items_approval_items_Id",
                        column: x => x.Id,
                        principalSchema: "cases",
                        principalTable: "approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "counselling_sessions",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AvrCaseId = table.Column<long>(type: "bigint", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "text", nullable: true, comment: "ISO 639-1 language code — string (standard, ανοιχτό set γλωσσών)"),
                    EnglishProficiency = table.Column<int>(type: "integer", nullable: false, comment: "Fluent | Some | None"),
                    InterpreterRequired = table.Column<bool>(type: "boolean", nullable: false),
                    InterpreterDetails = table.Column<string>(type: "text", nullable: true, comment: "Όνομα / γλώσσα / τρόπος — free text"),
                    CurrentStatus = table.Column<int>(type: "integer", nullable: false, comment: "AsylumSeeker | Student | Worker | Other"),
                    ReturnMotivation = table.Column<string>(type: "text", nullable: true, comment: "Long text — free text"),
                    EntryMethod = table.Column<int>(type: "integer", nullable: false, comment: "RegularCrossing | ThroughOccupiedArea | Other"),
                    EntryMethodDetails = table.Column<string>(type: "text", nullable: true, comment: "Free text (αν Other)"),
                    HowHeardAboutProgram = table.Column<int>(type: "integer", nullable: false, comment: "Office | FRONTEX | NGO | Friend | SocialMedia | Other"),
                    HoldsValidTravelDocument = table.Column<bool>(type: "boolean", nullable: false),
                    TravelDocumentLocation = table.Column<int>(type: "integer", nullable: true, comment: "WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld"),
                    LossOrTheftReported = table.Column<bool>(type: "boolean", nullable: false),
                    PoliceReportReference = table.Column<string>(type: "text", nullable: true, comment: "Αριθμός αναφοράς αστυνομίας — free text"),
                    HasCopyOfDocument = table.Column<bool>(type: "boolean", nullable: false),
                    HasPendingFinesOrDebts = table.Column<bool>(type: "boolean", nullable: false),
                    FinesDebtsDetails = table.Column<string>(type: "text", nullable: true, comment: "Λεπτομέρειες — free text"),
                    HasAsylumRejectionDecision = table.Column<bool>(type: "boolean", nullable: true),
                    AsylumAppealStatus = table.Column<int>(type: "integer", nullable: true, comment: "NoAppeal | Open | Closed"),
                    StudentVisaValid = table.Column<bool>(type: "boolean", nullable: true),
                    StudentVisaExpiry = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosedFileWithInstitution = table.Column<bool>(type: "boolean", nullable: true),
                    HoldsPinkSlip = table.Column<bool>(type: "boolean", nullable: true),
                    PinkSlipExpiry = table.Column<DateOnly>(type: "date", nullable: true),
                    HasEmployerReleasePaper = table.Column<bool>(type: "boolean", nullable: true),
                    OtherLegalSituation = table.Column<string>(type: "text", nullable: true, comment: "Free text (αν B1 = Other)"),
                    TravelGroup = table.Column<int>(type: "integer", nullable: false, comment: "Alone | WithSpouse | WithChildren | WithSpouseAndChildren | SingleParentWithChild"),
                    IsMarried = table.Column<bool>(type: "boolean", nullable: true),
                    OtherParentLocation = table.Column<string>(type: "text", nullable: true, comment: "Τοποθεσία άλλου γονέα — free text"),
                    HasFamilyInCyprus = table.Column<bool>(type: "boolean", nullable: true),
                    FamilyInCyprusDetails = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    FamilyPreviouslyReturned = table.Column<bool>(type: "boolean", nullable: true),
                    FamilyReturnDetails = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    VulnerabilityTypes = table.Column<int[]>(type: "integer[]", nullable: true, comment: "[Flags] enum — None/Pregnancy/Medical/Disability/UnaccompaniedMinor/Elderly/SingleParent/TraffickingVictim/MentalHealth/Other"),
                    NeedsAccommodation = table.Column<bool>(type: "boolean", nullable: false),
                    AccommodationDetails = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    HasMedicalCondition = table.Column<bool>(type: "boolean", nullable: false),
                    MedicalDetails = table.Column<string>(type: "text", nullable: true, comment: "Confidential — free text"),
                    NeedsAirportAssistance = table.Column<bool>(type: "boolean", nullable: false),
                    AirportAssistanceDetails = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    PreferredProgram = table.Column<int>(type: "integer", nullable: true, comment: "AVRCyprus | EURP | Undecided"),
                    WishesToProceed = table.Column<int>(type: "integer", nullable: false, comment: "Yes | No | StillConsidering"),
                    ContactPhone = table.Column<string>(type: "text", nullable: true, comment: "Τηλέφωνο — free text"),
                    CurrentAddress = table.Column<string>(type: "text", nullable: true, comment: "Διεύθυνση — free text"),
                    PreferredAirport = table.Column<string>(type: "text", nullable: true, comment: "IATA code ή free text (best effort)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counselling_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_counselling_sessions_avr_cases_AvrCaseId",
                        column: x => x.AvrCaseId,
                        principalSchema: "cases",
                        principalTable: "avr_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "by_own_return_decision_issuances",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ByOwnCaseId = table.Column<long>(type: "bigint", nullable: false),
                    DecisionId = table.Column<string>(type: "text", nullable: false, comment: "System-generated — string"),
                    DocumentName = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    DocumentLanguage = table.Column<int>(type: "integer", nullable: true),
                    DecisionFilePath = table.Column<string>(type: "text", nullable: true),
                    PreparedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    DecisionDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_by_own_return_decision_issuances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_by_own_return_decision_issuances_by_own_cases_ByOwnCaseId",
                        column: x => x.ByOwnCaseId,
                        principalSchema: "cases",
                        principalTable: "by_own_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_request_items",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseRequestId = table.Column<long>(type: "bigint", nullable: false),
                    IsReply = table.Column<bool>(type: "boolean", nullable: false, comment: "False = initial, True = reply"),
                    AuthorUserId = table.Column<long>(type: "bigint", nullable: false),
                    ItemDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Κείμενο — free text"),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_request_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_request_items_case_requests_CaseRequestId",
                        column: x => x.CaseRequestId,
                        principalSchema: "cases",
                        principalTable: "case_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_return_decisions",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseTcnId = table.Column<long>(type: "bigint", nullable: false),
                    DecisionId = table.Column<string>(type: "text", nullable: true, comment: "System-generated ID — string (human-readable reference)"),
                    Source = table.Column<int>(type: "integer", nullable: false, comment: "RECAMAS | ARS | CASS | Other"),
                    IssuingAuthority = table.Column<int>(type: "integer", nullable: false, comment: "MD | CAS | ARS | Other"),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TcnReceiptDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionText = table.Column<string>(type: "text", nullable: true, comment: "Κείμενο απόφασης — free text"),
                    VoluntaryReturnDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_return_decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_return_decisions_case_tcns_CaseTcnId",
                        column: x => x.CaseTcnId,
                        principalSchema: "cases",
                        principalTable: "case_tcns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_travel_documents",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseTcnId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true, comment: "Αριθμός εγγράφου — free text"),
                    IssuingCountryCode = table.Column<string>(type: "text", nullable: true, comment: "ISO 3166-1 alpha-2 — string (standard code)"),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true, comment: "Αρχή έκδοσης — free text (ανοιχτό set)"),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IssuedForReturnCase = table.Column<bool>(type: "boolean", nullable: false, comment: "True αν από Travel Doc Issuance process"),
                    CanBeUsedForReturn = table.Column<bool>(type: "boolean", nullable: false),
                    PhysicalLocation = table.Column<int>(type: "integer", nullable: true, comment: "WithApplicant | AtAsylumService | WithAuthorities | Lost | Stolen | NeverHeld | AIUOffice"),
                    Delivered = table.Column<bool>(type: "boolean", nullable: false),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true),
                    SourceIdentityDocumentId = table.Column<long>(type: "bigint", nullable: true, comment: "FK → IdentityDocument (αν από profile)"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_travel_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_travel_documents_case_tcns_CaseTcnId",
                        column: x => x.CaseTcnId,
                        principalSchema: "cases",
                        principalTable: "case_tcns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "security_findings",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SecurityDetailId = table.Column<long>(type: "bigint", nullable: false),
                    CaseTcnId = table.Column<long>(type: "bigint", nullable: true, comment: "FK → CaseTcn (αν εντοπίστηκε μέσα σε case)"),
                    FindingType = table.Column<int>(type: "integer", nullable: false, comment: "CriminalRecord | RestrictiveActivity | Other"),
                    SeverityLevel = table.Column<int>(type: "integer", nullable: false, comment: "Low | Medium | High"),
                    Details = table.Column<string>(type: "text", nullable: true, comment: "Ελεύθερο κείμενο περιγραφής — free text"),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true, comment: "Storage path"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_security_findings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_security_findings_case_tcns_CaseTcnId",
                        column: x => x.CaseTcnId,
                        principalSchema: "cases",
                        principalTable: "case_tcns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_security_findings_security_details_SecurityDetailId",
                        column: x => x.SecurityDetailId,
                        principalSchema: "tcn_profile",
                        principalTable: "security_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vulnerability_issues",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseTcnId = table.Column<long>(type: "bigint", nullable: false),
                    HealthIssueType = table.Column<int>(type: "integer", nullable: false, comment: "Medical | Disability | Pregnancy | Mental | Other"),
                    SeverityLevel = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Περιγραφή — free text"),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vulnerability_issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vulnerability_issues_case_tcns_CaseTcnId",
                        column: x => x.CaseTcnId,
                        principalSchema: "cases",
                        principalTable: "case_tcns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_counselling_sessions",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForcedReturnCaseId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: false, comment: "System-generated — string"),
                    SessionDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CounsellorType = table.Column<int>(type: "integer", nullable: false, comment: "AIUOfficer | FRONTEX | Other"),
                    CounsellorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    AttachmentPath = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "InProgress | Completed"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_counselling_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_counselling_sessions_forced_return_cases_ForcedRe~",
                        column: x => x.ForcedReturnCaseId,
                        principalSchema: "cases",
                        principalTable: "forced_return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_reassessments",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForcedReturnCaseId = table.Column<long>(type: "bigint", nullable: false),
                    ReassessmentType = table.Column<int>(type: "integer", nullable: false, comment: "Planned | AdHoc"),
                    PlannedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, comment: "Scheduled | Completed | Overdue"),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EvaluatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedIssueId = table.Column<long>(type: "bigint", nullable: true, comment: "FK → DetentionUpdateIssue"),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    AssessmentReportPath = table.Column<string>(type: "text", nullable: true, comment: "Storage path"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_reassessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_reassessments_forced_return_cases_ForcedReturnCas~",
                        column: x => x.ForcedReturnCaseId,
                        principalSchema: "cases",
                        principalTable: "forced_return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_records",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForcedReturnCaseId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    DetentionCenterId = table.Column<long>(type: "bigint", nullable: false),
                    DetentionWingId = table.Column<long>(type: "bigint", nullable: true),
                    DetentionRoomId = table.Column<long>(type: "bigint", nullable: true),
                    Bed = table.Column<string>(type: "text", nullable: true, comment: "[ΠΡΟΣΤΕΘΗΚΕ] — έλειπε, βλ. case_detention_period.bed PDF ref: §5.2.2 \"Detention Centers\" — \"the above metrics can also be applied to more detailed levels such as 'Wing' and 'Beds'\""),
                    DetentionStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DetentionEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CumulativeDetentionDays = table.Column<int>(type: "integer", nullable: true, comment: "Calculated"),
                    MaximumDetentionPeriodDays = table.Column<int>(type: "integer", nullable: true, comment: "Από admin parameter"),
                    IsInitialEntry = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_records_detention_centers_DetentionCenterId",
                        column: x => x.DetentionCenterId,
                        principalSchema: "detention",
                        principalTable: "detention_centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detention_records_forced_return_cases_ForcedReturnCaseId",
                        column: x => x.ForcedReturnCaseId,
                        principalSchema: "cases",
                        principalTable: "forced_return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detention_records_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_update_issues",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForcedReturnCaseId = table.Column<long>(type: "bigint", nullable: false),
                    IssueId = table.Column<string>(type: "text", nullable: false, comment: "System-generated — string"),
                    UpdateIssueType = table.Column<int>(type: "integer", nullable: false, comment: "Medical | YKERequest | TcnStatusUpdate | Other"),
                    IssueDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    RequiresReassessment = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresNewOrders = table.Column<bool>(type: "boolean", nullable: false),
                    BlocksDeparture = table.Column<bool>(type: "boolean", nullable: false),
                    AttachmentsPath = table.Column<string>(type: "text", nullable: true),
                    IsAutoGenerated = table.Column<bool>(type: "boolean", nullable: false, comment: "True αν από interface update"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_update_issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_update_issues_forced_return_cases_ForcedReturnCas~",
                        column: x => x.ForcedReturnCaseId,
                        principalSchema: "cases",
                        principalTable: "forced_return_cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_arrival_assistances",
                schema: "return_implementation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImplementationTcnId = table.Column<long>(type: "bigint", nullable: false),
                    Requested = table.Column<bool>(type: "boolean", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Provided = table.Column<bool>(type: "boolean", nullable: false),
                    AssistanceType = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    AttachmentsPath = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_arrival_assistances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_arrival_assistances_implementation_tcns_Implementation~",
                        column: x => x.ImplementationTcnId,
                        principalSchema: "return_implementation",
                        principalTable: "implementation_tcns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "escort_expenses",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EscortApprovalItemId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false, comment: "Transport | Accommodation | MedicalEquipment | Other"),
                    ExpenseAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escort_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_escort_expenses_escort_approval_items_EscortApprovalItemId",
                        column: x => x.EscortApprovalItemId,
                        principalSchema: "cases",
                        principalTable: "escort_approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monetary_incentive_approval_tcns",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MonetaryIncentiveApprovalItemId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monetary_incentive_approval_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monetary_incentive_approval_tcns_monetary_incentive_approva~",
                        column: x => x.MonetaryIncentiveApprovalItemId,
                        principalSchema: "cases",
                        principalTable: "monetary_incentive_approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_monetary_incentive_approval_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "return_decision_approval_tcns",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReturnDecisionApprovalItemId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_decision_approval_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_return_decision_approval_tcns_return_decision_approval_item~",
                        column: x => x.ReturnDecisionApprovalItemId,
                        principalSchema: "cases",
                        principalTable: "return_decision_approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_return_decision_approval_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "travel_doc_issuance_approval_tcns",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TravelDocIssuanceApprovalItemId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_travel_doc_issuance_approval_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_travel_doc_issuance_approval_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_travel_doc_issuance_approval_tcns_travel_doc_issuance_appro~",
                        column: x => x.TravelDocIssuanceApprovalItemId,
                        principalSchema: "cases",
                        principalTable: "travel_doc_issuance_approval_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "counselling_children",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CounsellingSessionId = table.Column<long>(type: "bigint", nullable: false),
                    HasValidTravelDocuments = table.Column<bool>(type: "boolean", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "text", nullable: true, comment: "Χώρα / πόλη γέννησης — free text"),
                    HasBirthCertificate = table.Column<bool>(type: "boolean", nullable: false),
                    HasConsentToTravel = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counselling_children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_counselling_children_counselling_sessions_CounsellingSessio~",
                        column: x => x.CounsellingSessionId,
                        principalSchema: "cases",
                        principalTable: "counselling_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "by_own_return_decision_tcns",
                schema: "cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ByOwnReturnDecisionIssuanceId = table.Column<long>(type: "bigint", nullable: false),
                    TcnProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_by_own_return_decision_tcns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_by_own_return_decision_tcns_by_own_return_decision_issuance~",
                        column: x => x.ByOwnReturnDecisionIssuanceId,
                        principalSchema: "cases",
                        principalTable: "by_own_return_decision_issuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_by_own_return_decision_tcns_tcn_profiles_TcnProfileId",
                        column: x => x.TcnProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "temporary_checkouts",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "PK — bigint / identity")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetentionRecordId = table.Column<long>(type: "bigint", nullable: false),
                    CheckoutDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedCheckinDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CheckoutReason = table.Column<int>(type: "integer", nullable: false, comment: "Medical | DoctorVisit | Embassy | Other"),
                    ActualCheckinDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Free text"),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Public identifier — UUIDv4 (§12.5.16)"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Χρόνος δημιουργίας εγγραφής"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false, comment: "User που δημιούργησε"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Χρόνος τελευταίας τροποποίησης"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User που τροποποίησε τελευταίος"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporary_checkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_temporary_checkouts_detention_records_DetentionRecordId",
                        column: x => x.DetentionRecordId,
                        principalSchema: "detention",
                        principalTable: "detention_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_additional_approvers_CaseId",
                schema: "cases",
                table: "additional_approvers",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_additional_approvers_PublicId",
                schema: "cases",
                table: "additional_approvers",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_notes_CaseId",
                schema: "cases",
                table: "adjustment_notes",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_adjustment_notes_PublicId",
                schema: "cases",
                table: "adjustment_notes",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_settings_PublicId",
                schema: "admin",
                table: "app_settings",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_settings_SettingKey",
                schema: "admin",
                table: "app_settings",
                column: "SettingKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appeals_PublicId",
                schema: "tcn_profile",
                table: "appeals",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appeals_TcnProfileId",
                schema: "tcn_profile",
                table: "appeals",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_approval_items_CaseId",
                schema: "cases",
                table: "approval_items",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_approval_items_PublicId",
                schema: "cases",
                table: "approval_items",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_arrival_departures_PublicId",
                schema: "tcn_profile",
                table: "arrival_departures",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_arrival_departures_TcnProfileId",
                schema: "tcn_profile",
                table: "arrival_departures",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AUDIT_OUTBOX_PICKUP",
                table: "audit_outbox",
                columns: new[] { "Status", "NextAttemptAt" });

            migrationBuilder.CreateIndex(
                name: "IX_business_rules_ClonedFromRuleId",
                schema: "admin",
                table: "business_rules",
                column: "ClonedFromRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_business_rules_PublicId",
                schema: "admin",
                table: "business_rules",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_business_rules_RuleId",
                schema: "admin",
                table: "business_rules",
                column: "RuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_by_own_return_decision_issuances_ByOwnCaseId",
                schema: "cases",
                table: "by_own_return_decision_issuances",
                column: "ByOwnCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_by_own_return_decision_issuances_PublicId",
                schema: "cases",
                table: "by_own_return_decision_issuances",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_by_own_return_decision_tcns_ByOwnReturnDecisionIssuanceId",
                schema: "cases",
                table: "by_own_return_decision_tcns",
                column: "ByOwnReturnDecisionIssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_by_own_return_decision_tcns_PublicId",
                schema: "cases",
                table: "by_own_return_decision_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_by_own_return_decision_tcns_TcnProfileId",
                schema: "cases",
                table: "by_own_return_decision_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_case_assignments_CaseId",
                schema: "cases",
                table: "case_assignments",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_case_assignments_PublicId",
                schema: "cases",
                table: "case_assignments",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_documents_CaseId",
                schema: "cases",
                table: "case_documents",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_case_documents_PublicId",
                schema: "cases",
                table: "case_documents",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_documents_TcnProfileId",
                schema: "cases",
                table: "case_documents",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_case_history_entries_CaseId",
                schema: "cases",
                table: "case_history_entries",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_case_history_entries_PublicId",
                schema: "cases",
                table: "case_history_entries",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_request_items_CaseRequestId",
                schema: "cases",
                table: "case_request_items",
                column: "CaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_case_request_items_PublicId",
                schema: "cases",
                table: "case_request_items",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_requests_CaseId",
                schema: "cases",
                table: "case_requests",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_case_requests_PublicId",
                schema: "cases",
                table: "case_requests",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_requests_RequestId",
                schema: "cases",
                table: "case_requests",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_return_decisions_CaseTcnId",
                schema: "cases",
                table: "case_return_decisions",
                column: "CaseTcnId");

            migrationBuilder.CreateIndex(
                name: "IX_case_return_decisions_PublicId",
                schema: "cases",
                table: "case_return_decisions",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_tcns_CaseId",
                schema: "cases",
                table: "case_tcns",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_case_tcns_PublicId",
                schema: "cases",
                table: "case_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_tcns_TcnProfileId",
                schema: "cases",
                table: "case_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_case_travel_documents_CaseTcnId",
                schema: "cases",
                table: "case_travel_documents",
                column: "CaseTcnId");

            migrationBuilder.CreateIndex(
                name: "IX_case_travel_documents_PublicId",
                schema: "cases",
                table: "case_travel_documents",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_codelist_values_CodelistId",
                schema: "admin",
                table: "codelist_values",
                column: "CodelistId");

            migrationBuilder.CreateIndex(
                name: "IX_codelist_values_PublicId",
                schema: "admin",
                table: "codelist_values",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_codelists_ListCode",
                schema: "admin",
                table: "codelists",
                column: "ListCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_codelists_PublicId",
                schema: "admin",
                table: "codelists",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_counselling_children_CounsellingSessionId",
                schema: "cases",
                table: "counselling_children",
                column: "CounsellingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_counselling_children_PublicId",
                schema: "cases",
                table: "counselling_children",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_counselling_sessions_AvrCaseId",
                schema: "cases",
                table: "counselling_sessions",
                column: "AvrCaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_counselling_sessions_PublicId",
                schema: "cases",
                table: "counselling_sessions",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_centers_PublicId",
                schema: "detention",
                table: "detention_centers",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_counselling_sessions_ForcedReturnCaseId",
                schema: "detention",
                table: "detention_counselling_sessions",
                column: "ForcedReturnCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_counselling_sessions_PublicId",
                schema: "detention",
                table: "detention_counselling_sessions",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_counselling_sessions_SessionId",
                schema: "detention",
                table: "detention_counselling_sessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_reassessments_ForcedReturnCaseId",
                schema: "detention",
                table: "detention_reassessments",
                column: "ForcedReturnCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_reassessments_PublicId",
                schema: "detention",
                table: "detention_reassessments",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_records_DetentionCenterId",
                schema: "detention",
                table: "detention_records",
                column: "DetentionCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_records_ForcedReturnCaseId",
                schema: "detention",
                table: "detention_records",
                column: "ForcedReturnCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_records_PublicId",
                schema: "detention",
                table: "detention_records",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_records_TcnProfileId",
                schema: "detention",
                table: "detention_records",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_rooms_DetentionWingId",
                schema: "detention",
                table: "detention_rooms",
                column: "DetentionWingId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_rooms_PublicId",
                schema: "detention",
                table: "detention_rooms",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_update_issues_ForcedReturnCaseId",
                schema: "detention",
                table: "detention_update_issues",
                column: "ForcedReturnCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_update_issues_IssueId",
                schema: "detention",
                table: "detention_update_issues",
                column: "IssueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_update_issues_PublicId",
                schema: "detention",
                table: "detention_update_issues",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detention_wings_DetentionCenterId",
                schema: "detention",
                table: "detention_wings",
                column: "DetentionCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_wings_PublicId",
                schema: "detention",
                table: "detention_wings",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_templates_PreviousVersionId",
                schema: "admin",
                table: "document_templates",
                column: "PreviousVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_document_templates_PublicId",
                schema: "admin",
                table: "document_templates",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_escort_expenses_EscortApprovalItemId",
                schema: "cases",
                table: "escort_expenses",
                column: "EscortApprovalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_escort_expenses_PublicId",
                schema: "cases",
                table: "escort_expenses",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_PublicId",
                schema: "tcn_profile",
                table: "identity_documents",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_TcnProfileId",
                schema: "tcn_profile",
                table: "identity_documents",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_documents_PublicId",
                schema: "return_implementation",
                table: "implementation_documents",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_implementation_documents_ReturnImplementationId",
                schema: "return_implementation",
                table: "implementation_documents",
                column: "ReturnImplementationId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_escort_expenses_ImplementationEscortTeamMemb~",
                schema: "return_implementation",
                table: "implementation_escort_expenses",
                column: "ImplementationEscortTeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_escort_expenses_PublicId",
                schema: "return_implementation",
                table: "implementation_escort_expenses",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_implementation_escort_team_members_PublicId",
                schema: "return_implementation",
                table: "implementation_escort_team_members",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_implementation_escort_team_members_ReturnImplementationId",
                schema: "return_implementation",
                table: "implementation_escort_team_members",
                column: "ReturnImplementationId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_other_expenses_PublicId",
                schema: "return_implementation",
                table: "implementation_other_expenses",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_implementation_other_expenses_ReturnImplementationId",
                schema: "return_implementation",
                table: "implementation_other_expenses",
                column: "ReturnImplementationId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_tcns_CaseId",
                schema: "return_implementation",
                table: "implementation_tcns",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_tcns_PublicId",
                schema: "return_implementation",
                table: "implementation_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_implementation_tcns_ReturnImplementationId",
                schema: "return_implementation",
                table: "implementation_tcns",
                column: "ReturnImplementationId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_tcns_TcnProfileId",
                schema: "return_implementation",
                table: "implementation_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_interface_sync_logs_PublicId",
                schema: "admin",
                table: "interface_sync_logs",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ip_applications_PublicId",
                schema: "tcn_profile",
                table: "ip_applications",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ip_applications_TcnProfileId",
                schema: "tcn_profile",
                table: "ip_applications",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ip_statuses_PublicId",
                schema: "tcn_profile",
                table: "ip_statuses",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ip_statuses_TcnProfileId",
                schema: "tcn_profile",
                table: "ip_statuses",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_linked_profiles_FromTcnProfileId",
                schema: "tcn_profile",
                table: "linked_profiles",
                column: "FromTcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_linked_profiles_PublicId",
                schema: "tcn_profile",
                table: "linked_profiles",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_linked_profiles_ToTcnProfileId",
                schema: "tcn_profile",
                table: "linked_profiles",
                column: "ToTcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_monetary_incentive_approval_tcns_MonetaryIncentiveApprovalI~",
                schema: "cases",
                table: "monetary_incentive_approval_tcns",
                column: "MonetaryIncentiveApprovalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_monetary_incentive_approval_tcns_PublicId",
                schema: "cases",
                table: "monetary_incentive_approval_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_monetary_incentive_approval_tcns_TcnProfileId",
                schema: "cases",
                table: "monetary_incentive_approval_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_PublicId",
                schema: "admin",
                table: "notifications",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_post_arrival_assistances_ImplementationTcnId",
                schema: "return_implementation",
                table: "post_arrival_assistances",
                column: "ImplementationTcnId");

            migrationBuilder.CreateIndex(
                name: "IX_post_arrival_assistances_PublicId",
                schema: "return_implementation",
                table: "post_arrival_assistances",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pre_return_checklists_CaseId",
                schema: "cases",
                table: "pre_return_checklists",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pre_return_checklists_PublicId",
                schema: "cases",
                table: "pre_return_checklists",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_residency_applications_PublicId",
                schema: "tcn_profile",
                table: "residency_applications",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_residency_applications_TcnProfileId",
                schema: "tcn_profile",
                table: "residency_applications",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_residency_statuses_PublicId",
                schema: "tcn_profile",
                table: "residency_statuses",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_residency_statuses_TcnProfileId",
                schema: "tcn_profile",
                table: "residency_statuses",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_return_cases_CaseId",
                schema: "cases",
                table: "return_cases",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_cases_PublicId",
                schema: "cases",
                table: "return_cases",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_cases_ReturnImplementationId",
                schema: "cases",
                table: "return_cases",
                column: "ReturnImplementationId");

            migrationBuilder.CreateIndex(
                name: "IX_return_decision_approval_tcns_PublicId",
                schema: "cases",
                table: "return_decision_approval_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_decision_approval_tcns_ReturnDecisionApprovalItemId",
                schema: "cases",
                table: "return_decision_approval_tcns",
                column: "ReturnDecisionApprovalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_return_decision_approval_tcns_TcnProfileId",
                schema: "cases",
                table: "return_decision_approval_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_return_decisions_IssuingCaseId",
                schema: "tcn_profile",
                table: "return_decisions",
                column: "IssuingCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_return_decisions_PublicId",
                schema: "tcn_profile",
                table: "return_decisions",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_decisions_TcnProfileId",
                schema: "tcn_profile",
                table: "return_decisions",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_return_implementations_ImplementationId",
                schema: "return_implementation",
                table: "return_implementations",
                column: "ImplementationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_implementations_PublicId",
                schema: "return_implementation",
                table: "return_implementations",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_security_details_PublicId",
                schema: "tcn_profile",
                table: "security_details",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_security_details_TcnProfileId",
                schema: "tcn_profile",
                table: "security_details",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_security_findings_CaseTcnId",
                schema: "tcn_profile",
                table: "security_findings",
                column: "CaseTcnId");

            migrationBuilder.CreateIndex(
                name: "IX_security_findings_PublicId",
                schema: "tcn_profile",
                table: "security_findings",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_security_findings_SecurityDetailId",
                schema: "tcn_profile",
                table: "security_findings",
                column: "SecurityDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_stoplist_entries_PublicId",
                schema: "tcn_profile",
                table: "stoplist_entries",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stoplist_entries_TcnProfileId",
                schema: "tcn_profile",
                table: "stoplist_entries",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_nationalities_PublicId",
                schema: "tcn_profile",
                table: "tcn_nationalities",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tcn_nationalities_TcnProfileId",
                schema: "tcn_profile",
                table: "tcn_nationalities",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_Arc",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "Arc");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_PublicId",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_RecamasId",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "RecamasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_temporary_checkouts_DetentionRecordId",
                schema: "detention",
                table: "temporary_checkouts",
                column: "DetentionRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_temporary_checkouts_PublicId",
                schema: "detention",
                table: "temporary_checkouts",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_travel_doc_issuance_approval_tcns_PublicId",
                schema: "cases",
                table: "travel_doc_issuance_approval_tcns",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_travel_doc_issuance_approval_tcns_TcnProfileId",
                schema: "cases",
                table: "travel_doc_issuance_approval_tcns",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_travel_doc_issuance_approval_tcns_TravelDocIssuanceApproval~",
                schema: "cases",
                table: "travel_doc_issuance_approval_tcns",
                column: "TravelDocIssuanceApprovalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_travel_document_issuances_CaseId",
                schema: "cases",
                table: "travel_document_issuances",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_travel_document_issuances_IssuanceId",
                schema: "cases",
                table: "travel_document_issuances",
                column: "IssuanceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_travel_document_issuances_PublicId",
                schema: "cases",
                table: "travel_document_issuances",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_travel_document_issuances_TcnProfileId",
                schema: "cases",
                table: "travel_document_issuances",
                column: "TcnProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_issues_CaseTcnId",
                schema: "cases",
                table: "vulnerability_issues",
                column: "CaseTcnId");

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_issues_PublicId",
                schema: "cases",
                table: "vulnerability_issues",
                column: "PublicId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "additional_approvers",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "adjustment_notes",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "alternative_measure_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "app_settings",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "appeals",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "arrival_departures",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "audit_outbox");

            migrationBuilder.DropTable(
                name: "business_rules",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "by_own_return_decision_tcns",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_assignments",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_documents",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_history_entries",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_request_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_return_decisions",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_travel_documents",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "codelist_values",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "counselling_children",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "detention_counselling_sessions",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "detention_reassessments",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "detention_rooms",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "detention_update_issues",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "document_templates",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "entry_ban_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "escort_expenses",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "frc_order_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "identity_documents",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "implementation_documents",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "implementation_escort_expenses",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "implementation_other_expenses",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "interface_sync_logs",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "ip_applications",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "ip_statuses",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "linked_profiles",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "monetary_incentive_approval_tcns",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "other_expense_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "post_arrival_assistances",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "pre_return_checklists",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "residency_applications",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "residency_statuses",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "return_decision_approval_tcns",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "return_decisions",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "security_findings",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "stoplist_entries",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_nationalities",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "temporary_checkouts",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "travel_doc_issuance_approval_tcns",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "travel_document_issuances",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "vulnerability_issues",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "by_own_return_decision_issuances",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_requests",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "codelists",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "counselling_sessions",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "detention_wings",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "escort_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "implementation_escort_team_members",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "monetary_incentive_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "implementation_tcns",
                schema: "return_implementation");

            migrationBuilder.DropTable(
                name: "return_decision_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "security_details",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "detention_records",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "travel_doc_issuance_approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "case_tcns",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "by_own_cases",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "avr_cases",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "detention_centers",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "forced_return_cases",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "approval_items",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "tcn_profiles",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "return_cases",
                schema: "cases");

            migrationBuilder.DropTable(
                name: "return_implementations",
                schema: "return_implementation");
        }
    }
}
