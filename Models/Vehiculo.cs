namespace EstacionamientoInteligente.Models;
public class Vehiculo
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public DateTime HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }

    public ICollection<Pago> Pagos { get; set; }
    public ICollection<Entrada> Entradas { get; set; }
    public ICollection<Estancia> Estancias { get; set; }
    public ICollection<Salida> Salidas { get; set; }
}