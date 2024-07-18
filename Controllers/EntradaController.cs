using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstacionamientoInteligente.Controllers
{
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
            // Normalizar la placa
            placa = placa?.ToUpper().Trim();

            if (string.IsNullOrEmpty(placa))
            {
                TempData["Error"] = "Por favor, ingrese una placa válida.";
                return RedirectToAction("Index");
            }

            // Verificar si ya existe un vehículo con esta placa en el estacionamiento
            var vehiculoExistente = await _context.Vehiculos
                .Include(v => v.Lugar)
                .FirstOrDefaultAsync(v => v.Placa == placa);

            // Buscar un lugar disponible
            var lugarDisponible = await _context.Lugares.FirstOrDefaultAsync(l => !l.Ocupado);

            if (lugarDisponible == null)
            {
                TempData["Error"] = "No hay lugares disponibles en el estacionamiento.";
                return RedirectToAction("Index");
            }

            if (vehiculoExistente != null)
            {
                if (vehiculoExistente.HoraSalida == null)
                {
                    TempData["Error"] = $"El vehículo con placa {placa} ya está en el estacionamiento desde {vehiculoExistente.HoraEntrada}.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // El vehículo existe pero había salido, actualizamos su registro
                    vehiculoExistente.HoraEntrada = DateTime.Now;
                    vehiculoExistente.HoraSalida = null;
                    vehiculoExistente.Lugar = lugarDisponible;
                    vehiculoExistente.LugarId = lugarDisponible.Id;
                }
            }
            else
            {
                // Crear nuevo vehículo
                vehiculoExistente = new Vehiculo
                {
                    Placa = placa,
                    HoraEntrada = DateTime.Now,
                    Lugar = lugarDisponible,
                    LugarId = lugarDisponible.Id
                };
                _context.Vehiculos.Add(vehiculoExistente);
            }

            // Actualizar el lugar
            lugarDisponible.Ocupado = true;
            lugarDisponible.Vehiculo = vehiculoExistente;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"El vehículo con placa {placa} ha ingresado al estacionamiento.";
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details here
                TempData["Error"] = "Hubo un error al registrar el vehículo. Por favor, inténtelo de nuevo.";
            }

            return RedirectToAction("Index");
        }
    }
}
