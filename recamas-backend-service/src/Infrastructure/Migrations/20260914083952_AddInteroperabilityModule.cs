using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RECAMAS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInteroperabilityModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_stoplist_entries_TcnProfileId",
                schema: "tcn_profile",
                table: "stoplist_entries");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CheckedAt",
                schema: "tcn_profile",
                table: "stoplist_entries",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Χρόνος εκτέλεσης του Stoplist check");

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "DurationMilliseconds",
                schema: "admin",
                table: "interface_sync_logs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorCode",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalReference",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HttpStatusCode",
                schema: "admin",
                table: "interface_sync_logs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Operation",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestPayloadHash",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsePayloadHash",
                schema: "admin",
                table: "interface_sync_logs",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExternalPersonId",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExternalRecordId",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalStatusCode",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LinkedDepartureExternalId",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportIssuingCountryCode",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "integer",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.AddColumn<string>(
                name: "VisaNumber",
                schema: "tcn_profile",
                table: "arrival_departures",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stoplist_entries_TcnProfileId_CheckedAt",
                schema: "tcn_profile",
                table: "stoplist_entries",
                columns: new[] { "TcnProfileId", "CheckedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_interface_sync_logs_CorrelationId",
                schema: "admin",
                table: "interface_sync_logs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_interface_sync_logs_ExternalSystem_StartedAt",
                schema: "admin",
                table: "interface_sync_logs",
                columns: new[] { "ExternalSystem", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_interface_sync_logs_Status_StartedAt",
                schema: "admin",
                table: "interface_sync_logs",
                columns: new[] { "Status", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_arrival_departures_Source_MovementType_ExternalRecordId",
                schema: "tcn_profile",
                table: "arrival_departures",
                columns: new[] { "Source", "MovementType", "ExternalRecordId" },
                unique: true,
                filter: "\"ExternalRecordId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_stoplist_entries_TcnProfileId_CheckedAt",
                schema: "tcn_profile",
                table: "stoplist_entries");

            migrationBuilder.DropIndex(
                name: "IX_interface_sync_logs_CorrelationId",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropIndex(
                name: "IX_interface_sync_logs_ExternalSystem_StartedAt",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropIndex(
                name: "IX_interface_sync_logs_Status_StartedAt",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropIndex(
                name: "IX_arrival_departures_Source_MovementType_ExternalRecordId",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "CheckedAt",
                schema: "tcn_profile",
                table: "stoplist_entries");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "DurationMilliseconds",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "ErrorCode",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "ExternalReference",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "HttpStatusCode",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "Operation",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "RequestPayloadHash",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "ResponsePayloadHash",
                schema: "admin",
                table: "interface_sync_logs");

            migrationBuilder.DropColumn(
                name: "ExternalPersonId",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "ExternalRecordId",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "ExternalStatusCode",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "LinkedDepartureExternalId",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "PassportIssuingCountryCode",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "Source",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.DropColumn(
                name: "VisaNumber",
                schema: "tcn_profile",
                table: "arrival_departures");

            migrationBuilder.CreateIndex(
                name: "IX_stoplist_entries_TcnProfileId",
                schema: "tcn_profile",
                table: "stoplist_entries",
                column: "TcnProfileId");
        }
    }
}
