using EstacionamientoInteligente.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EstacionamientoInteligente.Controllers;
using EstacionamientoInteligente.Data;
public class EstanciaController : Controller
{
    private readonly EstacionamientoContext _context;

    public EstanciaController(EstacionamientoContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var lugares = await _context.Lugares.Include(l => l.Vehiculo).ToListAsync();
        return View(lugares);
    }

    [HttpPost]
    public async Task<IActionResult> OcuparLugar(int lugarId, string placa)
    {
        var lugar = await _context.Lugares.FindAsync(lugarId);
        if (lugar != null && !lugar.Ocupado)
        {
            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa && v.HoraSalida == null);
            if (vehiculo != null)
            {
                lugar.Ocupado = true;
                lugar.VehiculoId = vehiculo.Id;
                await _context.SaveChangesAsync();
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
