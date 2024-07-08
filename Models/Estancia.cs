using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models;

public class Estancia
{
    public int Id { get; set; }

    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; }

    public int LugarId { get; set; }
    public Lugar Lugar { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    public bool EstaActiva { get; set; }
}