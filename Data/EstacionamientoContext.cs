using Microsoft.EntityFrameworkCore;
using EstacionamientoInteligente.Models;

namespace EstacionamientoInteligente.Data;

public class EstacionamientoContext : DbContext
{
    public EstacionamientoContext(DbContextOptions<EstacionamientoContext> options)
        : base(options)
    { }

    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Lugar> Lugares { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<Estancia> Estancias { get; set; }
    public DbSet<Salida> Salidas { get; set; }
    public DbSet<ListadoCobro> ListadoCobros { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Pago>(entity =>
    {
        entity.Property(p => p.Monto)
            .HasColumnType("decimal(18, 2)"); // Reemplácese con la precisión y escala adecuadas
    });
}
}
