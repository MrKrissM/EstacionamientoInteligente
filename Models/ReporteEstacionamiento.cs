
namespace EstacionamientoInteligente.Models;

public class ReporteEstacionamiento
{
    public string Placa { get; set; }
    public DateTime HoraEntrada { get; set; }
    public DateTime HoraSalida { get; set; }
    public int MinutosEstacionado { get; set; }
    public decimal MontoCobrado { get; set; }
}