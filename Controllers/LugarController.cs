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
    public class LugarController : Controller
    {
        private readonly EstacionamientoContext _context;

        public LugarController(EstacionamientoContext context)
        {
            _context = context;
            InitializeLugares().Wait();
        }

        private async Task InitializeLugares()
        {
            var lugaresExistentes = await _context.Lugares.ToListAsync();
            var numerosExistentes = lugaresExistentes.Select(l => l.Numero).ToHashSet();

            for (int i = 1; i <= 20; i++)
            {
                if (!numerosExistentes.Contains(i))
                {
                    _context.Lugares.Add(new Lugar { Numero = i, Ocupado = false });
                }
            }

            if (lugaresExistentes.Count > 20)
            {
                var lugaresExcedentes = lugaresExistentes.OrderBy(l => l.Numero).Skip(20);
                _context.Lugares.RemoveRange(lugaresExcedentes);
            }

            await _context.SaveChangesAsync();
        }

        // GET: Lugar
        public async Task<IActionResult> Index()
        {
            var lugares = await _context.Lugares
                .Include(l => l.Vehiculo)
                .OrderBy(l => l.Numero)
                .Take(20)
                .ToListAsync();
            return View(lugares);
        }

        // GET: Lugar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lugar = await _context.Lugares
                .Include(l => l.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lugar == null)
            {
                return NotFound();
            }

            return View(lugar);
        }

        // GET: Lugar/Create
        public IActionResult Create()
        {
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id");
            return View();
        }

        // POST: Lugar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ocupado,VehiculoId")] Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lugar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", lugar.VehiculoId);
            return View(lugar);
        }

        // GET: Lugar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lugar = await _context.Lugares.FindAsync(id);
            if (lugar == null)
            {
                return NotFound();
            }
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", lugar.VehiculoId);
            return View(lugar);
        }

        // POST: Lugar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ocupado,VehiculoId")] Lugar lugar)
        {
            if (id != lugar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lugar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LugarExists(lugar.Id))
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
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", lugar.VehiculoId);
            return View(lugar);
        }

        // GET: Lugar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lugar = await _context.Lugares
                .Include(l => l.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lugar == null)
            {
                return NotFound();
            }

            return View(lugar);
        }

        // POST: Lugar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);
            if (lugar != null)
            {
                _context.Lugares.Remove(lugar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LugarExists(int id)
        {
            return _context.Lugares.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MarcarOcupado(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);
            if (lugar != null)
            {
                lugar.Ocupado = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarcarLibre(int id)
        {
            var lugar = await _context.Lugares
                .Include(l => l.Vehiculo)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lugar == null)
            {
                TempData["Error"] = "El lugar de estacionamiento no existe.";
                return RedirectToAction(nameof(Index));
            }

            if (!lugar.Ocupado)
            {
                TempData["Error"] = "Este lugar ya está libre.";
                return RedirectToAction(nameof(Index));
            }

            if (lugar.Vehiculo != null)
            {
                lugar.Vehiculo.HoraSalida = DateTime.Now;
                lugar.Vehiculo = null;
            }

            lugar.Ocupado = false;
            lugar.VehiculoId = null;

            await _context.SaveChangesAsync();

            TempData["Exito"] = "El lugar ha sido marcado como libre y el vehículo ha salido.";
            return RedirectToAction(nameof(Index));
        }
    }

}
