using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kierowcy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumerPrawaJazdy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kierowcy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pojazdy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumerRejestracyjny = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Przebieg = table.Column<decimal>(type: "decimal(12,1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PojemnoscZbiornika = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Typ = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Ladownosc = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    LiczbaMiejsc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pojazdy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tankowania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IloscLitrow = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CenaZaLitr = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tankowania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tankowania_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_NumerRejestracyjny",
                table: "Pojazdy",
                column: "NumerRejestracyjny",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_PojazdId",
                table: "Tankowania",
                column: "PojazdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kierowcy");

            migrationBuilder.DropTable(
                name: "Tankowania");

            migrationBuilder.DropTable(
                name: "Pojazdy");
        }
    }
}
