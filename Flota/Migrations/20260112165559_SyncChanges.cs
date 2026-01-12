using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class SyncChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ladownosc",
                table: "Pojazdy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ladownosc",
                table: "Pojazdy",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
