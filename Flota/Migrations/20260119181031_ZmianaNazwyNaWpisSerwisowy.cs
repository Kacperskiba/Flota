using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class ZmianaNazwyNaWpisSerwisowy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZgloszeniaSerwisowe_Pojazdy_PojazdId",
                table: "ZgloszeniaSerwisowe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZgloszeniaSerwisowe",
                table: "ZgloszeniaSerwisowe");

            migrationBuilder.RenameTable(
                name: "ZgloszeniaSerwisowe",
                newName: "WpisSerwisowy");

            migrationBuilder.RenameIndex(
                name: "IX_ZgloszeniaSerwisowe_PojazdId",
                table: "WpisSerwisowy",
                newName: "IX_WpisSerwisowy_PojazdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WpisSerwisowy",
                table: "WpisSerwisowy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WpisSerwisowy_Pojazdy_PojazdId",
                table: "WpisSerwisowy",
                column: "PojazdId",
                principalTable: "Pojazdy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WpisSerwisowy_Pojazdy_PojazdId",
                table: "WpisSerwisowy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WpisSerwisowy",
                table: "WpisSerwisowy");

            migrationBuilder.RenameTable(
                name: "WpisSerwisowy",
                newName: "ZgloszeniaSerwisowe");

            migrationBuilder.RenameIndex(
                name: "IX_WpisSerwisowy_PojazdId",
                table: "ZgloszeniaSerwisowe",
                newName: "IX_ZgloszeniaSerwisowe_PojazdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZgloszeniaSerwisowe",
                table: "ZgloszeniaSerwisowe",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ZgloszeniaSerwisowe_Pojazdy_PojazdId",
                table: "ZgloszeniaSerwisowe",
                column: "PojazdId",
                principalTable: "Pojazdy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
