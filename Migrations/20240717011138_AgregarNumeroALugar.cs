using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroALugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estancias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estancias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LugarId = table.Column<int>(type: "int", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false),
                    EstaActiva = table.Column<bool>(type: "bit", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estancias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estancias_Lugares_LugarId",
                        column: x => x.LugarId,
                        principalTable: "Lugares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estancias_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estancias_LugarId",
                table: "Estancias",
                column: "LugarId");

            migrationBuilder.CreateIndex(
                name: "IX_Estancias_VehiculoId",
                table: "Estancias",
                column: "VehiculoId");
        }
    }
}
