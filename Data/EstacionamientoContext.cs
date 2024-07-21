using Microsoft.EntityFrameworkCore;
using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EstacionamientoInteligente.Data;

public class EstacionamientoContext : IdentityDbContext<ApplicationUser>
{
    public EstacionamientoContext(DbContextOptions<EstacionamientoContext> options)
        : base(options)
    { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Lugar> Lugares { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<Salida> Salidas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Definir una clave primaria para IdentityUserLogin<string>
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
        modelBuilder.Entity<Usuario>()
               .HasIndex(u => u.Username)
               .IsUnique();

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.Property(p => p.Monto)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Lugar>()
            .HasIndex(l => l.Numero)
            .IsUnique();


        modelBuilder.Entity<Vehiculo>()
            .HasOne(v => v.Lugar)
            .WithOne(l => l.Vehiculo)
            .HasForeignKey<Vehiculo>(v => v.LugarId)
            .IsRequired(false);

    }
}
