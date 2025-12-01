using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CitySubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PollingIntervalMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NextPollAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitySubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitySubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherMeasurements_CitySubscriptions_CitySubscriptionId",
                        column: x => x.CitySubscriptionId,
                        principalTable: "CitySubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CitySubscriptions_CityName",
                table: "CitySubscriptions",
                column: "CityName");

            migrationBuilder.CreateIndex(
                name: "IX_CitySubscriptions_IsActive_NextPollAt",
                table: "CitySubscriptions",
                columns: new[] { "IsActive", "NextPollAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherMeasurements_CitySubscriptionId_Timestamp",
                table: "WeatherMeasurements",
                columns: new[] { "CitySubscriptionId", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherMeasurements");

            migrationBuilder.DropTable(
                name: "CitySubscriptions");
        }
    }
}
