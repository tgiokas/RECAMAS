using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RECAMAS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleSkeletonsAndExternalClients : Migration
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
                name: "voluntary_return_own_means_case_details",
                schema: "case");

            migrationBuilder.DropTable(
                name: "detention_orders",
                schema: "detention");

            migrationBuilder.DropTable(
                name: "rules",
                schema: "rules");

            migrationBuilder.DropTable(
                name: "cases",
                schema: "case");

            migrationBuilder.DropTable(
                name: "detention_facilities",
                schema: "detention");
        }
    }
}
