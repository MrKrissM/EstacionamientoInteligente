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
                .FirstOrDefaultAsync(v => v.Placa == placa && v.HoraSalida == null);

            if (vehiculoExistente != null)
            {
                TempData["Error"] = $"El vehículo con placa {placa} ya está en el estacionamiento desde {vehiculoExistente.HoraEntrada}.";
                return RedirectToAction("Index");
            }

            // Buscar un lugar disponible
            var lugarDisponible = await _context.Lugares.FirstOrDefaultAsync(l => !l.Ocupado);

            if (lugarDisponible == null)
            {
                TempData["Error"] = "No hay lugares disponibles en el estacionamiento.";
                return RedirectToAction("Index");
            }

            // Crear nuevo vehículo y asignar lugar
            var nuevoVehiculo = new Vehiculo
            {
                Placa = placa,
                HoraEntrada = DateTime.Now
            };

            lugarDisponible.Ocupado = true;
            lugarDisponible.Vehiculo = nuevoVehiculo;

            _context.Vehiculos.Add(nuevoVehiculo);

            try
            {
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"El vehículo con placa {placa} ha ingresado al estacionamiento.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Hubo un error al registrar el vehículo. Por favor, inténtelo de nuevo.";
            }

            return RedirectToAction("Index");
        }
    }
}
