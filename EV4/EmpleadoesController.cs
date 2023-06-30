using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EV4.Data;
using EV4.Models;

namespace EV4
{
    public class EmpleadoesController : Controller
    {
        private readonly EVContext _context;

        public EmpleadoesController(EVContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            return _context.Empleado != null ?
                View(await _context.Empleado.ToListAsync()) :
                Problem("Entity set 'EVContext.Empleado' is null.");
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleado == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Email,FechaContratacion")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexOrdenado));
            }
            return View(empleado);
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleado == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Email,FechaContratacion")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexOrdenado));
            }
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleado == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleado == null)
            {
                return Problem("Entity set 'EVContext.Empleado' is null.");
            }

            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexOrdenado));
        }

        private bool EmpleadoExists(int id)
        {
            return (_context.Empleado?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> IndexOrdenado(string sortOrder)
        {
            // Obtener los valores actuales de ordenación para cada columna
            string currentNombreSort = ViewData["NombreSortParam"] as string;
            string currentApellidoSort = ViewData["ApellidoSortParam"] as string;
            string currentEmailSort = ViewData["EmailSortParam"] as string;
            string currentFechaContratacionSort = ViewData["FechaContratacionSortParam"] as string;

            // Determinar la dirección de ordenación (ascendente o descendente) para la columna seleccionada
            string nombreSort = "";
            string apellidoSort = "";
            string emailSort = "";
            string fechaContratacionSort = "";

            if (string.IsNullOrEmpty(sortOrder))
            {
                nombreSort = "nombre_asc";
                apellidoSort = "apellido_asc";
                emailSort = "email_asc";
                fechaContratacionSort = "fechaContratacion_asc";
            }
            else
            {
                nombreSort = sortOrder == "nombre_asc" ? "nombre_desc" : "nombre_asc";
                apellidoSort = sortOrder == "apellido_asc" ? "apellido_desc" : "apellido_asc";
                emailSort = sortOrder == "email_asc" ? "email_desc" : "email_asc";
                fechaContratacionSort = sortOrder == "fechaContratacion_asc" ? "fechaContratacion_desc" : "fechaContratacion_asc";
            }

            // Actualizar los valores de ordenación en ViewData
            ViewData["NombreSortParam"] = nombreSort;
            ViewData["ApellidoSortParam"] = apellidoSort;
            ViewData["EmailSortParam"] = emailSort;
            ViewData["FechaContratacionSortParam"] = fechaContratacionSort;

            var empleados = _context.Empleado.AsQueryable();

            switch (sortOrder)
            {
                case "nombre_asc":
                    empleados = empleados.OrderBy(e => e.Nombre);
                    break;
                case "nombre_desc":
                    empleados = empleados.OrderByDescending(e => e.Nombre);
                    break;
                case "apellido_asc":
                    empleados = empleados.OrderBy(e => e.Apellido);
                    break;
                case "apellido_desc":
                    empleados = empleados.OrderByDescending(e => e.Apellido);
                    break;
                case "email_asc":
                    empleados = empleados.OrderBy(e => e.Email);
                    break;
                case "email_desc":
                    empleados = empleados.OrderByDescending(e => e.Email);
                    break;
                case "fechaContratacion_asc":
                    empleados = empleados.OrderBy(e => e.FechaContratacion);
                    break;
                case "fechaContratacion_desc":
                    empleados = empleados.OrderByDescending(e => e.FechaContratacion);
                    break;
                default:
                    empleados = empleados.OrderBy(e => e.Nombre);
                    break;
            }

            return View("IndexOrdenado", await empleados.ToListAsync());
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction(nameof(IndexOrdenado));
            }

            var empleados = _context.Empleado
                .Where(e => e.Nombre.Contains(searchTerm) ||
                            e.Apellido.Contains(searchTerm) ||
                            e.Email.Contains(searchTerm) ||
                            e.FechaContratacion.ToString().Contains(searchTerm));

            return View("IndexOrdenado", await empleados.ToListAsync());
        }

    }
}
