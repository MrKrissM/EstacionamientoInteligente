using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;
using EstacionamientoInteligente.Helpers;

namespace EstacionamientoInteligente.Controllers
{
    public class PagoController : Controller
    {
        private readonly EstacionamientoContext _context;

        public PagoController(EstacionamientoContext context)
        {
            _context = context;
        }

        // GET: Pago
        public async Task<IActionResult> Index()
        {
            var estacionamientoContext = _context.Pagos.Include(p => p.Vehiculo);
            return View(await estacionamientoContext.ToListAsync());
        }

        // GET: Pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            // ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id");
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Placa,Monto,FechaPago")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                // Find the vehicle by Placa
                var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == pago.Placa);

                if (vehiculo != null)
                {
                    pago.VehiculoId = vehiculo.Id; // Set VehiculoId based on found vehicle
                    _context.Add(pago);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle case where vehicle with Placa is not found
                    ModelState.AddModelError("Placa", "Placa no encontrada");
                }
            }

            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", pago.VehiculoId);
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehiculoId,Monto,FechaPago")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", pago.VehiculoId);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MarcarSalida(string placa) // Use string for placa
        {
            if (string.IsNullOrEmpty(placa))
            {
                ModelState.AddModelError("Placa", "La placa del vehículo es requerida");
                return View(); // Consider returning a specific "Salida" view
            }

            // Check for existing vehicle with the same placa (case-insensitive)
            var existingVehiculo = await _context.Vehiculos
                                                 .FirstOrDefaultAsync(v => v.Placa.ToLower() == placa.ToLower());

            if (existingVehiculo != null)
            {
                // Handle duplicate plate scenario
                ModelState.AddModelError("Placa", "Placa duplicada. Se han eliminado todos los registros con esta placa.");

                // Delete all vehicles with the duplicate placa (assuming you want to prevent duplicates)
                _context.Vehiculos.RemoveRange(_context.Vehiculos.Where(v => v.Placa.ToLower() == placa.ToLower()));
                await _context.SaveChangesAsync();

                return View(); // Consider returning a specific "Salida" view with a warning message
            }

            // If no duplicate found, proceed with normal salida logic
            var vehiculo = new Vehiculo { Placa = placa, HoraSalida = DateTime.Now }; // Assuming you create a new record

            _context.Add(vehiculo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to desired action
        }

        // Controllers/PagoController.cs
        // Controllers/PagoController.cs

        public async Task<IActionResult> GenerarReporte(DateTime fecha)
        {
            var reporte = await _context.Pagos
                .Include(p => p.Vehiculo)
                .Where(p => p.FechaPago.Date == fecha.Date)
                .Select(p => new ReporteEstacionamiento
                {
                    Placa = p.Vehiculo.Placa,
                    HoraEntrada = p.Vehiculo.HoraEntrada,
                    HoraSalida = p.Vehiculo.HoraSalida ?? DateTime.Now,
                    MinutosEstacionado = p.MinutosEstacionado,
                    MontoCobrado = p.Monto
                })
                .ToListAsync();

            byte[] pdfBytes = PdfHelper.GenerarReportePdf(reporte, fecha);

            return File(pdfBytes, "application/pdf", $"ReporteEstacionamiento_{fecha:yyyyMMdd}.pdf");
        }

    }
}
