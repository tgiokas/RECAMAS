using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RECAMAS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompleteTCNProfileModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NoCriminalRecordFound",
                schema: "tcn_profile",
                table: "tcn_profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoRestrictiveActivitiesFound",
                schema: "tcn_profile",
                table: "tcn_profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tcn_appeals",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_arrivals_departures",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_international_protection_applications",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_international_protection_statuses",
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

            migrationBuilder.DropColumn(
                name: "NoCriminalRecordFound",
                schema: "tcn_profile",
                table: "tcn_profiles");

            migrationBuilder.DropColumn(
                name: "NoRestrictiveActivitiesFound",
                schema: "tcn_profile",
                table: "tcn_profiles");
        }
    }
}
