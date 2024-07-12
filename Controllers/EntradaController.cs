using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstacionamientoInteligente.Controllers;

public class EntradaController : Controller
{
    private readonly EstacionamientoContext _context;

    public EntradaController(EstacionamientoContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarEntrada(string placa)
    {
        var vehiculo = new Vehiculo {Id = 0, Placa = placa, HoraEntrada = DateTime.Now };
        _context.Vehiculos.Add(vehiculo);
   

        // Find an available lugar
        var lugarDisponible = await _context.Lugares.FirstOrDefaultAsync(l => !l.Ocupado);
        if (lugarDisponible != null)
        {
            lugarDisponible.Ocupado = true;
            lugarDisponible.VehiculoId = vehiculo.Id;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}