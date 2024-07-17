namespace EstacionamientoInteligente.Models;
public class Lugar
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public bool Ocupado { get; set; }
    public int? VehiculoId { get; set; }
    public Vehiculo Vehiculo { get; set; }
    
}