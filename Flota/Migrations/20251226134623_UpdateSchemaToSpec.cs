using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaToSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataZatrudnienia",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Kierowcy");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ladownosc",
                table: "Pojazdy",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RokProdukcji",
                table: "Pojazdy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Kierowcy",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Kierowcy",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LataDoswiadczenia",
                table: "Kierowcy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NrTelefonu",
                table: "Kierowcy",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Przydzialy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    KierowcaId = table.Column<int>(type: "int", nullable: false),
                    DataRozpoczecia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZakonczenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrzebiegPoczatkowy = table.Column<decimal>(type: "decimal(12,1)", nullable: false),
                    PrzebiegKoncowy = table.Column<decimal>(type: "decimal(12,1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przydzialy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Przydzialy_Kierowcy_KierowcaId",
                        column: x => x.KierowcaId,
                        principalTable: "Kierowcy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Przydzialy_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZgloszeniaSerwisowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Koszt = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DataZgloszenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZgloszeniaSerwisowe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZgloszeniaSerwisowe_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kierowcy_NumerPrawaJazdy",
                table: "Kierowcy",
                column: "NumerPrawaJazdy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Przydzialy_KierowcaId",
                table: "Przydzialy",
                column: "KierowcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Przydzialy_PojazdId",
                table: "Przydzialy",
                column: "PojazdId");

            migrationBuilder.CreateIndex(
                name: "IX_ZgloszeniaSerwisowe_PojazdId",
                table: "ZgloszeniaSerwisowe",
                column: "PojazdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Przydzialy");

            migrationBuilder.DropTable(
                name: "ZgloszeniaSerwisowe");

            migrationBuilder.DropIndex(
                name: "IX_Kierowcy_NumerPrawaJazdy",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "RokProdukcji",
                table: "Pojazdy");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "LataDoswiadczenia",
                table: "Kierowcy");

            migrationBuilder.DropColumn(
                name: "NrTelefonu",
                table: "Kierowcy");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ladownosc",
                table: "Pojazdy",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Kierowcy",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataZatrudnienia",
                table: "Kierowcy",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Kierowcy",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
