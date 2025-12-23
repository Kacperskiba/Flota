using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKierowca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataZatrudnienia",
                table: "Kierowcy",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Kierowcy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Kierowcy",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataZatrudnienia",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Kierowcy");
        }
    }
}
