using System;
using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models;

public class Salida
{
    public int Id { get; set; }

    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; }

    public DateTime FechaHoraSalida { get; set; }

    public decimal TiempoEstancia { get; set; } // En minutos

    public decimal MontoAPagar { get; set; }

    public bool PagoRealizado { get; set; }

    public int? PagoId { get; set; }
    public Pago Pago { get; set; }
}