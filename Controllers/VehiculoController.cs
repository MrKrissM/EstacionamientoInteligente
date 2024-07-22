using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstacionamientoInteligente.Data;
using EstacionamientoInteligente.Models;

namespace EstacionamientoInteligente.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly EstacionamientoContext _context;

        public VehiculoController(EstacionamientoContext context)
        {
            _context = context;
        }

        // GET: Vehiculo
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _context.Vehiculos
                .Where(v => v.HoraSalida == null)
                .OrderBy(v => v.HoraEntrada)
                .ToListAsync();
            return View(await _context.Vehiculos.ToListAsync());
        }

        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehiculo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Placa,HoraEntrada,HoraSalida")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();

                // Buscar un lugar disponible
                var lugarDisponible = await _context.Lugares.FirstOrDefaultAsync(l => !l.Ocupado);
                if (lugarDisponible != null)
                {
                    lugarDisponible.Ocupado = true;
                    lugarDisponible.VehiculoId = vehiculo.Id;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vehiculo);
        }

        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Placa,HoraEntrada,HoraSalida")] Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Id))
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
            return View(vehiculo);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> RegistrarSalida(string placa)
        {
            if (string.IsNullOrEmpty(placa))
            {
                TempData["Error"] = "Por favor, ingrese una placa válida.";
                return RedirectToAction("Index", "Lugar");
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Lugar)
                .FirstOrDefaultAsync(v => v.Placa == placa && v.HoraSalida == null);

            if (vehiculo == null)
            {
                TempData["Error"] = $"No se encontró un vehículo con la placa {placa} en el estacionamiento.";
                return RedirectToAction("Index", "Lugar");
            }

            vehiculo.HoraSalida = DateTime.Now;

            // Calcular el tiempo de estancia y el monto a pagar
            TimeSpan tiempoEstancia = vehiculo.HoraSalida.Value - vehiculo.HoraEntrada;
            int minutos = (int)tiempoEstancia.TotalMinutes;
            decimal monto = minutos * 20; // 20 pesos por minuto

            if (vehiculo.Lugar != null)
            {
                vehiculo.Lugar.Ocupado = false;
                vehiculo.Lugar.VehiculoId = null;
                vehiculo.Lugar = null;
            }

            vehiculo.LugarId = null;

            await _context.SaveChangesAsync();

            // Crear un nuevo pago
            var pago = new Pago
            {
                VehiculoId = vehiculo.Id,
                Monto = monto,
                FechaPago = vehiculo.HoraSalida.Value,
                Placa = vehiculo.Placa,
                MinutosEstacionado = minutos
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return RedirectToAction("MostrarSalida", new { id = pago.Id });
        }

        public async Task<IActionResult> MostrarSalida(int id)
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
}
