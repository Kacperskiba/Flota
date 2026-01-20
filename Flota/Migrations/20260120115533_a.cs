using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flota.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "LacznyKoszt",
                table: "Tankowania",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IloscLitrow",
                table: "Tankowania",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CenaZaLitr",
                table: "Tankowania",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<int>(
                name: "KierowcaId",
                table: "Tankowania",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_KierowcaId",
                table: "Tankowania",
                column: "KierowcaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tankowania_Kierowcy_KierowcaId",
                table: "Tankowania",
                column: "KierowcaId",
                principalTable: "Kierowcy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tankowania_Kierowcy_KierowcaId",
                table: "Tankowania");

            migrationBuilder.DropIndex(
                name: "IX_Tankowania_KierowcaId",
                table: "Tankowania");

            migrationBuilder.DropColumn(
                name: "KierowcaId",
                table: "Tankowania");

            migrationBuilder.AlterColumn<decimal>(
                name: "LacznyKoszt",
                table: "Tankowania",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IloscLitrow",
                table: "Tankowania",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CenaZaLitr",
                table: "Tankowania",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
