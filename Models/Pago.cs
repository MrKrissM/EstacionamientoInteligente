namespace EstacionamientoInteligente.Models;
public class Pago
{
    public int Id { get; set; }
    public int VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
   
}