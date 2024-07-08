using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models;

public class Entrada
{
    public int Id { get; set; }

    [Required]
    public string Placa { get; set; }

    public DateTime FechaHoraEntrada { get; set; }

    public int? LugarAsignadoId { get; set; }
    public Lugar LugarAsignado { get; set; }

    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; }
}