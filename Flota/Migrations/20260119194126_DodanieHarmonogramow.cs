using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class DodanieHarmonogramow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Harmonogramy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    InterwalKm = table.Column<int>(type: "int", nullable: false),
                    InterwalDni = table.Column<int>(type: "int", nullable: false),
                    DataOstatniegoPrzegladu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrzebiegOstatniegoPrzegladu = table.Column<decimal>(type: "decimal(12,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harmonogramy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Harmonogramy_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Harmonogramy_PojazdId",
                table: "Harmonogramy",
                column: "PojazdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Harmonogramy");
        }
    }
}
