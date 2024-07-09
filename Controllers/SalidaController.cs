using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;
namespace EstacionamientoInteligente.Controllers;

public class SalidaController : Controller
{
    private readonly EstacionamientoContext _context;

    public SalidaController(EstacionamientoContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarSalida(string placa)
    {
        var vehiculo = await _context.Vehiculos
            .FirstOrDefaultAsync(v => v.Placa == placa && v.HoraSalida == null);

        if (vehiculo != null)
        {
            vehiculo.HoraSalida = DateTime.Now;
            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.VehiculoId == vehiculo.Id);
            if (lugar != null)
            {
                lugar.Ocupado = false;
                lugar.VehiculoId = null;
            }

            // Cálculo simple de tarifa (asumiendo $1 por minuto)
            var duracion = (vehiculo.HoraSalida.Value - vehiculo.HoraEntrada).TotalMinutes;
            var tarifa = (decimal)duracion;

            var pago = new Pago
            {
                VehiculoId = vehiculo.Id,
                Monto = tarifa,
                FechaPago = DateTime.Now
            };
            _context.Pagos.Add(pago);

            await _context.SaveChangesAsync();
            return RedirectToAction("Pago", new { id = pago.Id });
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Pago(int id)
    {
        var pago = await _context.Pagos
            .Include(p => p.Vehiculo)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pago == null)
        {
            return NotFound();
        }

        return View(pago);
    }
}
