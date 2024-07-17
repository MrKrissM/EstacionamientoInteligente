﻿// <auto-generated />
using System;
using EstacionamientoInteligente.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EstacionamientoInteligente.Migrations
{
    [DbContext(typeof(EstacionamientoContext))]
    [Migration("20240717023944_AgregarNumeroALugarLugar")]
    partial class AgregarNumeroALugarLugar
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EstacionamientoInteligente.Models.Entrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaHoraEntrada")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LugarAsignadoId")
                        .HasColumnType("int");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LugarAsignadoId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("Entradas");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Lugar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.Property<bool>("Ocupado")
                        .HasColumnType("bit");

                    b.Property<int?>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehiculoId")
                        .IsUnique()
                        .HasFilter("[VehiculoId] IS NOT NULL");

                    b.ToTable("Lugares");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Pago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaPago")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehiculoId");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Salida", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaHoraSalida")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MontoAPagar")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PagoId")
                        .HasColumnType("int");

                    b.Property<bool>("PagoRealizado")
                        .HasColumnType("bit");

                    b.Property<decimal>("TiempoEstancia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PagoId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("Salidas");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Vehiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("HoraEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("HoraSalida")
                        .HasColumnType("datetime2");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Entrada", b =>
                {
                    b.HasOne("EstacionamientoInteligente.Models.Lugar", "LugarAsignado")
                        .WithMany()
                        .HasForeignKey("LugarAsignadoId");

                    b.HasOne("EstacionamientoInteligente.Models.Vehiculo", "Vehiculo")
                        .WithMany("Entradas")
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LugarAsignado");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Lugar", b =>
                {
                    b.HasOne("EstacionamientoInteligente.Models.Vehiculo", "Vehiculo")
                        .WithOne("Lugar")
                        .HasForeignKey("EstacionamientoInteligente.Models.Lugar", "VehiculoId");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Pago", b =>
                {
                    b.HasOne("EstacionamientoInteligente.Models.Vehiculo", "Vehiculo")
                        .WithMany("Pagos")
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Salida", b =>
                {
                    b.HasOne("EstacionamientoInteligente.Models.Pago", "Pago")
                        .WithMany()
                        .HasForeignKey("PagoId");

                    b.HasOne("EstacionamientoInteligente.Models.Vehiculo", "Vehiculo")
                        .WithMany("Salidas")
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pago");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("EstacionamientoInteligente.Models.Vehiculo", b =>
                {
                    b.Navigation("Entradas");

                    b.Navigation("Lugar")
                        .IsRequired();

                    b.Navigation("Pagos");

                    b.Navigation("Salidas");
                });
#pragma warning restore 612, 618
        }
    }
}
