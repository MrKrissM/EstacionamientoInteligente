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
    public DbSet<Salida> Salidas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Pago>(entity =>
    {
        entity.Property(p => p.Monto)
            .HasColumnType("decimal(18, 2)");
    });

    modelBuilder.Entity<Lugar>()
        .HasOne(l => l.Vehiculo)
        .WithOne(v => v.Lugar)
        .HasForeignKey<Lugar>(l => l.VehiculoId)
        .OnDelete(DeleteBehavior.SetNull);

    modelBuilder.Entity<Vehiculo>()
        .HasOne(v => v.Lugar)
        .WithOne(l => l.Vehiculo)
        .HasForeignKey<Vehiculo>(v => v.LugarId)
        .OnDelete(DeleteBehavior.SetNull);
}
}
