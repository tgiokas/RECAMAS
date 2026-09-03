using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RECAMAS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialTCNProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tcn_profile");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

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
                name: "IX_tcn_nationalities_TCNProfileId",
                schema: "tcn_profile",
                table: "tcn_nationalities",
                column: "TCNProfileId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tcn_identity_documents",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_nationalities",
                schema: "tcn_profile");

            migrationBuilder.DropTable(
                name: "tcn_profiles",
                schema: "tcn_profile");
        }
    }
}
