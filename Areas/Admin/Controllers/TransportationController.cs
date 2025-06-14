using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TransportationController : Controller
    {
        private readonly TourDbContext _context;

        public TransportationController(TourDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Transportation
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            var items = from t in _context.Transportations
                        select t;

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                items = items.Where(t =>
                    t.Name.Contains(searchString) ||
                    (t.Provider != null && t.Provider.Contains(searchString)));
            }

            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TypeSort"] = sortOrder == "type" ? "type_desc" : "type";

            items = sortOrder switch
            {
                "name_desc" => items.OrderByDescending(t => t.Name),
                "type" => items.OrderBy(t => t.Type),
                "type_desc" => items.OrderByDescending(t => t.Type),
                _ => items.OrderBy(t => t.Name),
            };

            var list = await items.AsNoTracking().ToListAsync();
            return View(list);
        }

        // GET: Admin/Transportation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var transport = await _context.Transportations
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID_Transport == id);
            if (transport == null) return NotFound();
            return View(transport);
        }

        // GET: Admin/Transportation/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Admin/Transportation/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Type,Name,Capacity,Provider,License_Plate,Description")] Transportation transport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transport);
        }

        // GET: Admin/Transportation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var transport = await _context.Transportations.FindAsync(id);
            if (transport == null) return NotFound();
            return View(transport);
        }

        // POST: Admin/Transportation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Transport,Type,Name,Capacity,Provider,License_Plate,Description")] Transportation transport)
        {
            if (id != transport.ID_Transport) return NotFound();
            if (!ModelState.IsValid) return View(transport);

            try
            {
                _context.Update(transport);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Transportations.AnyAsync(e => e.ID_Transport == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Transportation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var transport = await _context.Transportations
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID_Transport == id);
            if (transport == null) return NotFound();
            return View(transport);
        }

        // POST: Admin/Transportation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transport = await _context.Transportations.FindAsync(id);
            if (transport != null)
            {
                _context.Transportations.Remove(transport);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}