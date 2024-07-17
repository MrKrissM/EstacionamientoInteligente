using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehiculoLugarRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_Vehiculos_VehiculoId",
                table: "Lugares");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_Vehiculos_VehiculoId1",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Lugares_VehiculoId1",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "VehiculoId1",
                table: "Lugares");

            migrationBuilder.AddColumn<int>(
                name: "LugarId",
                table: "Vehiculos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_LugarId",
                table: "Vehiculos",
                column: "LugarId",
                unique: true,
                filter: "[LugarId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos",
                column: "LugarId",
                principalTable: "Lugares",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Lugares_LugarId",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_LugarId",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "LugarId",
                table: "Vehiculos");

            migrationBuilder.AddColumn<int>(
                name: "VehiculoId1",
                table: "Lugares",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_VehiculoId",
                table: "Lugares",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_VehiculoId1",
                table: "Lugares",
                column: "VehiculoId1",
                unique: true,
                filter: "[VehiculoId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_Vehiculos_VehiculoId",
                table: "Lugares",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_Vehiculos_VehiculoId1",
                table: "Lugares",
                column: "VehiculoId1",
                principalTable: "Vehiculos",
                principalColumn: "Id");
        }
    }
}
