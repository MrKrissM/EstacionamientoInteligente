using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Mvc;

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
        var vehiculo = new Vehiculo { Placa = placa, HoraEntrada = DateTime.Now };
        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}