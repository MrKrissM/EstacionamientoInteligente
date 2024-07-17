namespace EstacionamientoInteligente.Models;
public class Vehiculo
{

    public int Id { get; set; }
    public string Placa { get; set; }
    public DateTime HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
    public int? LugarId { get; set; }
    public Lugar Lugar { get; set; }


    public ICollection<Pago> Pagos { get; set; }
    public ICollection<Entrada> Entradas { get; set; }
    public ICollection<Salida> Salidas { get; set; }

}