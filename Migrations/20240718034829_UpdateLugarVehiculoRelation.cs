using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLugarVehiculoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos",
                column: "LugarId",
                principalTable: "Lugares",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos",
                column: "LugarId",
                principalTable: "Lugares",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
