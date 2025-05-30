// using Microsoft.AspNetCore.Mvc;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using LAPTRINHWEB.Data;
// using LAPTRINHWEB.Models;
// using Microsoft.EntityFrameworkCore;

// namespace LAPTRINHWEB.Controllers
// {
//     public class TourController : Controller
//     {
//         private readonly TourDbContext _context;
        
//         public TourController(TourDbContext context)
//         {
//             _context = context;
//         }
        
//         public async Task<IActionResult> Index()
//         {
//             var tours = await _context.Tours
//                 .Where(t => t.Status == TourStatus.Active)
//                 .ToListAsync();
//             return View(tours);
//         }
        
//         public async Task<IActionResult> Detail(int id)
//         {
//             var tour = await _context.Tours
//                 .FirstOrDefaultAsync(t => t.ID_Tour == id);
            
//             if (tour == null)
//             {
//                 return NotFound();
//             }
            
//             return View(tour);
//         }
        
//         // API để lấy tour theo AJAX
//         [HttpGet]
//         public async Task<JsonResult> GetTours()
//         {
//             var tours = await _context.Tours
//                 .Where(t => t.Status == TourStatus.Active)
//                 .Select(t => new {
//                     id = t.ID_Tour,
//                     name = t.Name_Tour,
//                     price = t.Price,
//                     discount = t.Discount,
//                     finalPrice = t.FinalPrice,
//                     duration = t.Duration,
//                     startLocation = t.Start_Location,
//                     endLocation = t.End_Location
//                 })
//                 .ToListAsync();
            
//             return Json(tours);
//         }
//     }
// }



