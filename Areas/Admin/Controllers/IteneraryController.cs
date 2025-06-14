using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IteneraryController : Controller
    {
        private readonly TourDbContext _context;
        public IteneraryController(TourDbContext context) => _context = context;

        // GET: Admin/Itenerary
        public async Task<IActionResult> Index()
        {
            var list = await _context.Itineraries
                                     .Include(i => i.Tour)
                                     .Include(i => i.Details)
                                     .OrderBy(i => i.Tour.Name_Tour)
                                     .ThenBy(i => i.Day_Number)
                                     .AsNoTracking()
                                     .ToListAsync();
            return View(list);
        }

        // GET: Admin/Itenerary/Add
        public IActionResult Add()
        {
            ViewData["TourList"] = new SelectList(
                _context.Tours.AsNoTracking(),
                "ID_Tour", "Name_Tour"
            );
            ViewData["LocationList"] = new MultiSelectList(
                _context.Locations.AsNoTracking(),
                "ID_Location", "Name"
            );
            return View();
        }

        // POST: Admin/Itenerary/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("ID_Tour,Day_Number,Title,Details")] Itinerary model,
            int[] SelectedLocations)
        {
            if (ModelState.IsValid)
            {
                _context.Itineraries.Add(model);
                await _context.SaveChangesAsync();

                if (SelectedLocations != null && SelectedLocations.Any())
                {
                    foreach (var locId in SelectedLocations)
                    {
                        _context.ItineraryLocations.Add(new ItineraryLocation
                        {
                            ID_Itinerary = model.ID_Itinerary,
                            ID_Location = locId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourList"] = new SelectList(_context.Tours.AsNoTracking(), "ID_Tour", "Name_Tour", model.ID_Tour);
            ViewData["LocationList"] = new MultiSelectList(_context.Locations.AsNoTracking(), "ID_Location", "Name");
            return View(model);
        }

        // GET: Admin/Itenerary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            var item = await _context.Itineraries
                                     .Include(i => i.Details)
                                     .Include(i => i.ItineraryLocations)
                                     .FirstOrDefaultAsync(i => i.ID_Itinerary == id.Value);
            if (item == null)
                return NotFound();
            ViewData["TourList"] = new SelectList(
                _context.Tours.AsNoTracking(),
                "ID_Tour", "Name_Tour", item.ID_Tour
            );
            ViewData["LocationList"] = new MultiSelectList(
                _context.Locations.AsNoTracking(),
                "ID_Location", "Name",
                item.ItineraryLocations.Select(il => il.ID_Location)
            );
            return View(item);
        }

        // POST: Admin/Itenerary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID_Itinerary,ID_Tour,Day_Number,Title,Details")] Itinerary model,
            int[] SelectedLocations)
        {
            if (id != model.ID_Itinerary)
                return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["TourList"] = new SelectList(
                    _context.Tours.AsNoTracking(),
                    "ID_Tour", "Name_Tour", model.ID_Tour
                );
                ViewData["LocationList"] = new MultiSelectList(
                    _context.Locations.AsNoTracking(),
                    "ID_Location", "Name", SelectedLocations
                );
                return View(model);
            }
            try
            {
                var itineraryToUpdate = await _context.Itineraries
                    .Include(i => i.Details)
                    .FirstOrDefaultAsync(i => i.ID_Itinerary == id);
                if (itineraryToUpdate == null)
                    return NotFound();

                itineraryToUpdate.ID_Tour = model.ID_Tour;
                itineraryToUpdate.Day_Number = model.Day_Number;
                itineraryToUpdate.Title = model.Title;

                // Cập nhật collection Details: xóa các detail cũ, sau đó thêm mới từ model.Details
                _context.RemoveRange(itineraryToUpdate.Details);
                if (model.Details != null)
                {
                    foreach (var detail in model.Details)
                    {
                        itineraryToUpdate.Details.Add(new ItineraryDetail
                        {
                            Time = detail.Time,
                            Activities = detail.Activities
                        });
                    }
                }

                // Cập nhật các liên kết với Locations
                var existingLinks = _context.ItineraryLocations.Where(il => il.ID_Itinerary == id);
                _context.ItineraryLocations.RemoveRange(existingLinks);
                await _context.SaveChangesAsync();

                if (SelectedLocations != null && SelectedLocations.Any())
                {
                    foreach (var locId in SelectedLocations)
                    {
                        _context.ItineraryLocations.Add(new ItineraryLocation
                        {
                            ID_Itinerary = itineraryToUpdate.ID_Itinerary,
                            ID_Location = locId
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Itineraries.AnyAsync(e => e.ID_Itinerary == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Itenerary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return NotFound();
            var item = await _context.Itineraries
                                     .Include(i => i.Tour)
                                     .Include(i => i.Details)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(i => i.ID_Itinerary == id.Value);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: Admin/Itenerary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Itineraries.FindAsync(id);
            if (item != null)
            {
                _context.Itineraries.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}