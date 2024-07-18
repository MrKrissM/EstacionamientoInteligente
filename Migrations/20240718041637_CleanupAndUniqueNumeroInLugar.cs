using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    /// <inheritdoc />
public partial class CleanupAndUniqueNumeroInLugar : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Primero, eliminamos los registros duplicados
        migrationBuilder.Sql(@"
            WITH CTE AS (
                SELECT 
                    *,
                    ROW_NUMBER() OVER (PARTITION BY Numero ORDER BY Id) AS RowNum
                FROM Lugares
            )
            DELETE FROM CTE WHERE RowNum > 1
        ");

        // Luego, nos aseguramos de que tengamos exactamente 20 lugares numerados del 1 al 20
        migrationBuilder.Sql(@"
            ;WITH CTE AS (
                SELECT TOP 20 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Numero
                FROM sys.objects
            )
            MERGE INTO Lugares AS target
            USING CTE AS source ON target.Numero = source.Numero
            WHEN NOT MATCHED BY TARGET THEN
                INSERT (Numero, Ocupado) VALUES (source.Numero, 0)
            WHEN NOT MATCHED BY SOURCE THEN
                DELETE;
        ");

        // Finalmente, añadimos la restricción única
        migrationBuilder.CreateIndex(
            name: "IX_Lugares_Numero",
            table: "Lugares",
            column: "Numero",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Lugares_Numero",
            table: "Lugares");
    }
}
}
