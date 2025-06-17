using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Data;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourGuideController : Controller
    {
        private readonly TourDbContext _context;

        public TourGuideController(TourDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TourGuide
        public async Task<IActionResult> Index()
        {
            var guides = await _context.TourGuides.ToListAsync();
            return View(guides);
        }

        // Thêm các action khác cho controller quản lý TourGuide
    }
}