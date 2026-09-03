using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RECAMAS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "case");

            migrationBuilder.EnsureSchema(
                name: "detention");

            migrationBuilder.EnsureSchema(
                name: "reports");

            migrationBuilder.EnsureSchema(
                name: "return_impl");

            migrationBuilder.EnsureSchema(
                name: "rules");

            migrationBuilder.EnsureSchema(
                name: "tcn_profile");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

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
                name: "avr_case_details",
                schema: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avr_case_details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cases",
                schema: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayCode = table.Column<string>(type: "text", nullable: true),
                    CaseType = table.Column<int>(type: "integer", nullable: false),
                    Program = table.Column<string>(type: "text", nullable: true),
                    Stage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    InitiationDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    InitiationOffice = table.Column<string>(type: "text", nullable: true),
                    ImplementationOffice = table.Column<string>(type: "text", nullable: true),
                    ReturnCountry = table.Column<string>(type: "text", nullable: true),
                    ReturnReason = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "detention_facilities",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TotalCapacity = table.Column<int>(type: "integer", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "document_templates",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseType = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TemplateFileDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "forced_return_case_details",
                schema: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forced_return_case_details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rules",
                schema: "rules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CaseType = table.Column<int>(type: "integer", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tcn_profiles",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayCode = table.Column<string>(type: "text", nullable: true),
                    Arc = table.Column<string>(type: "text", nullable: true),
                    FirstNameEl = table.Column<string>(type: "text", nullable: true),
                    FirstNameEn = table.Column<string>(type: "text", nullable: true),
                    MiddleNameEl = table.Column<string>(type: "text", nullable: true),
                    MiddleNameEn = table.Column<string>(type: "text", nullable: true),
                    LastNameEl = table.Column<string>(type: "text", nullable: true),
                    LastNameEn = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "text", nullable: true),
                    MdFileNo = table.Column<string>(type: "text", nullable: true),
                    RelationshipToMdFile = table.Column<int>(type: "integer", nullable: true),
                    CassFileNo = table.Column<string>(type: "text", nullable: true),
                    CassAddress = table.Column<string>(type: "text", nullable: true),
                    CassPhone = table.Column<string>(type: "text", nullable: true),
                    MdAddress = table.Column<string>(type: "text", nullable: true),
                    MdPhone = table.Column<string>(type: "text", nullable: true),
                    EurodacNumber = table.Column<string>(type: "text", nullable: true),
                    NoCriminalRecordFound = table.Column<bool>(type: "boolean", nullable: false),
                    NoRestrictiveActivitiesFound = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "voluntary_return_own_means_case_details",
                schema: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voluntary_return_own_means_case_details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "return_implementations",
                schema: "return_impl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    DepartureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FlightNumber = table.Column<string>(type: "text", nullable: true),
                    DepartureConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_implementations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_return_implementations_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "case",
                        principalTable: "cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "detention_orders",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    DetentionFacilityId = table.Column<long>(type: "bigint", nullable: true),
                    DetentionStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DetentionEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_orders_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "case",
                        principalTable: "cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_detention_orders_detention_facilities_DetentionFacilityId",
                        column: x => x.DetentionFacilityId,
                        principalSchema: "detention",
                        principalTable: "detention_facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rule_versions",
                schema: "rules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    ConditionsJson = table.Column<string>(type: "jsonb", nullable: false),
                    ThenActionsJson = table.Column<string>(type: "jsonb", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rule_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rule_versions_rules_RuleId",
                        column: x => x.RuleId,
                        principalSchema: "rules",
                        principalTable: "rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_tcn_profiles",
                schema: "case",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_tcn_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_case_tcn_profiles_cases_CaseId",
                        column: x => x.CaseId,
                        principalSchema: "case",
                        principalTable: "cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_case_tcn_profiles_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tcn_appeals",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfAppeal = table.Column<string>(type: "text", nullable: true),
                    AppealNumber = table.Column<string>(type: "text", nullable: true),
                    AppealDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AppealStatusDecision = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_appeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_appeals_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_arrivals_departures",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Airport = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_arrivals_departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_arrivals_departures_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_identity_documents",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    IsTravelDocument = table.Column<bool>(type: "boolean", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true),
                    IssuingCountry = table.Column<string>(type: "text", nullable: true),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AttachmentDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_identity_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_identity_documents_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_international_protection_applications",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfApplication = table.Column<string>(type: "text", nullable: true),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StatusDecision = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_international_protection_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_international_protection_applications_tcn_profiles_TCNP~",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_international_protection_statuses",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfStatus = table.Column<string>(type: "text", nullable: true),
                    DateOfGranting = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StatusDecision = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_international_protection_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_international_protection_statuses_tcn_profiles_TCNProfi~",
                        column: x => x.TCNProfileId,
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    NationalityCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    IdentificationStatus = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_nationalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_nationalities_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_profile_links",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    LinkedProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_profile_links", x => x.Id);
                    table.CheckConstraint("CK_tcn_profile_links_not_self", "\"TCNProfileId\" <> \"LinkedProfileId\"");
                    table.ForeignKey(
                        name: "FK_tcn_profile_links_tcn_profiles_LinkedProfileId",
                        column: x => x.LinkedProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tcn_profile_links_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_residency_applications",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeOfPermitRequested = table.Column<string>(type: "text", nullable: true),
                    TypeOfApplication = table.Column<string>(type: "text", nullable: true),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResidenceCategory = table.Column<string>(type: "text", nullable: true),
                    PurposeOfResidenceRnd = table.Column<string>(type: "text", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_residency_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_residency_applications_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_residency_statuses",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    PermitType = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResidenceCategory = table.Column<string>(type: "text", nullable: true),
                    PurposeOfResidenceRnd = table.Column<string>(type: "text", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    ResidencyDocumentNumber = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_residency_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_residency_statuses_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_return_decisions",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true),
                    DecisionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionText = table.Column<string>(type: "text", nullable: true),
                    TcnReceiptDate = table.Column<DateOnly>(type: "date", nullable: true),
                    VoluntaryReturnDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true),
                    DecisionFileDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_return_decisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_return_decisions_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_security_findings",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    FindingType = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    AttachmentDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_security_findings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_security_findings_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tcn_stoplist_entries",
                schema: "tcn_profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCNProfileId = table.Column<long>(type: "bigint", nullable: false),
                    StoplistHit = table.Column<bool>(type: "boolean", nullable: false),
                    StoplistReason = table.Column<string>(type: "text", nullable: true),
                    UniqueEntryBanNumber = table.Column<string>(type: "text", nullable: true),
                    StoplistEntryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryBanDurationMonths = table.Column<int>(type: "integer", nullable: true),
                    EntryBanExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcn_stoplist_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tcn_stoplist_entries_tcn_profiles_TCNProfileId",
                        column: x => x.TCNProfileId,
                        principalSchema: "tcn_profile",
                        principalTable: "tcn_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detention_reassessments",
                schema: "detention",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetentionOrderId = table.Column<long>(type: "bigint", nullable: false),
                    MilestoneMonths = table.Column<int>(type: "integer", nullable: false),
                    ScheduledDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Outcome = table.Column<string>(type: "text", nullable: true),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detention_reassessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detention_reassessments_detention_orders_DetentionOrderId",
                        column: x => x.DetentionOrderId,
                        principalSchema: "detention",
                        principalTable: "detention_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUDIT_OUTBOX_PICKUP",
                table: "audit_outbox",
                columns: new[] { "Status", "NextAttemptAt" });

            migrationBuilder.CreateIndex(
                name: "IX_avr_case_details_CaseId",
                schema: "case",
                table: "avr_case_details",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_tcn_profiles_CaseId_TCNProfileId",
                schema: "case",
                table: "case_tcn_profiles",
                columns: new[] { "CaseId", "TCNProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_case_tcn_profiles_TCNProfileId",
                schema: "case",
                table: "case_tcn_profiles",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_cases_DisplayCode",
                schema: "case",
                table: "cases",
                column: "DisplayCode",
                unique: true,
                filter: "\"DisplayCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cases_PublicId",
                schema: "case",
                table: "cases",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cases_Status_CaseType",
                schema: "case",
                table: "cases",
                columns: new[] { "Status", "CaseType" });

            migrationBuilder.CreateIndex(
                name: "IX_detention_orders_CaseId",
                schema: "detention",
                table: "detention_orders",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_orders_DetentionFacilityId",
                schema: "detention",
                table: "detention_orders",
                column: "DetentionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_detention_reassessments_DetentionOrderId",
                schema: "detention",
                table: "detention_reassessments",
                column: "DetentionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_document_templates_CaseType_Title",
                schema: "reports",
                table: "document_templates",
                columns: new[] { "CaseType", "Title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_forced_return_case_details_CaseId",
                schema: "case",
                table: "forced_return_case_details",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_return_implementations_CaseId",
                schema: "return_impl",
                table: "return_implementations",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_rule_versions_RuleId_VersionNumber",
                schema: "rules",
                table: "rule_versions",
                columns: new[] { "RuleId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tcn_appeals_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_appeals",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_arrivals_departures_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_arrivals_departures",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_identity_documents_DocumentNumber",
                schema: "tcn_profile",
                table: "tcn_identity_documents",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_identity_documents_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_identity_documents",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_international_protection_applications_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_international_protection_applications",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_international_protection_statuses_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_international_protection_statuses",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_nationalities_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_nationalities",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profile_links_LinkedProfileId",
                schema: "tcn_profile",
                table: "tcn_profile_links",
                column: "LinkedProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profile_links_TCNProfileId_LinkedProfileId",
                schema: "tcn_profile",
                table: "tcn_profile_links",
                columns: new[] { "TCNProfileId", "LinkedProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_Arc",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "Arc");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_Arc_FirstNameEn_LastNameEn",
                schema: "tcn_profile",
                table: "tcn_profiles",
                columns: new[] { "Arc", "FirstNameEn", "LastNameEn" })
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops", "gin_trgm_ops", "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_DisplayCode",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "DisplayCode",
                unique: true,
                filter: "\"DisplayCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_profiles_PublicId",
                schema: "tcn_profile",
                table: "tcn_profiles",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tcn_residency_applications_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_residency_applications",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_residency_statuses_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_residency_statuses",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_return_decisions_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_return_decisions",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_security_findings_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_security_findings",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_stoplist_entries_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_stoplist_entries",
                column: "TCNProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_tcn_stoplist_entries_UniqueEntryBanNumber",
                schema: "tcn_profile",
                table: "tcn_stoplist_entries",
                column: "UniqueEntryBanNumber");

            migrationBuilder.CreateIndex(
                name: "IX_voluntary_return_own_means_case_details_CaseId",
                schema: "case",
                table: "voluntary_return_own_means_case_details",
                column: "CaseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_outbox");

            migrationBuilder.DropTable(
                name: "avr_case_details",
                schema: "case");

            migrationBuilder.DropTable(
                name: "case_tcn_profiles",
                schema: "case");

            migrationBuilder.DropTable(
                name: "detention_reassessments",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "document_templates",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "forced_return_case_details",
                schema: "case");

            migrationBuilder.DropTable(
                name: "return_implementations",
                schema: "return_impl");

            migrationBuilder.DropTable(
                name: "rule_versions",
                schema: "rules");

            migrationBuilder.DropTable(
                name: "tcn_appeals",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_arrivals_departures",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_identity_documents",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_international_protection_applications",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_international_protection_statuses",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_nationalities",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_profile_links",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_residency_applications",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_residency_statuses",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_return_decisions",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_security_findings",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_stoplist_entries",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "voluntary_return_own_means_case_details",
                schema: "case");

            migrationBuilder.DropTable(
                name: "detention_orders",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "rules",
                schema: "rules");

            migrationBuilder.DropTable(
                name: "tcn_profiles",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "cases",
                schema: "case");

            migrationBuilder.DropTable(
                name: "detention_facilities",
                schema: "detention");
        }
    }
}
