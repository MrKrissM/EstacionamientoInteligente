using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroALugarLugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares");

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares",
                column: "VehiculoId",
                unique: true,
                filter: "[VehiculoId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares");

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares",
                column: "VehiculoId");
        }
    }
}
