using Microsoft.AspNetCore.Mvc;
using LAPTRINHWEB.Models; // Đảm bảo đã thêm using cho Name_Tourspace Models
using System.Collections.Generic;
using System.Linq;
using System;

namespace LAPTRINHWEB.Controllers
{
    public class TourController : Controller
    {
        // public IActionResult Detail()
        // {
        //     return View();
        // }
        // Tạo danh sách mock data cho Tour
        private static List<Tour> _mockTours = new List<Tour>
        {
            new Tour
            {
                ID_Tour = 1,
                Name_Tour = "Đà Lạt - Thành phố ngàn hoa",
                Description = "Khám phá vẻ đẹp thơ mộng của Đà Lạt với khí hậu mát mẻ quanh năm, những đồi thông xanh mướt và các loài hoa rực rỡ.",
                Price = 2990000,
                StartDate = new DateTime(2025, 6, 15),
                EndDate = new DateTime(2025, 6, 17),
                NumberOfPeople  = 20,
                NumberOfDays = 3,
                NumberOfNights = 2,
                Type_Tour = "Tham quan, Nghỉ dưỡng"
            },
            new Tour
            {
                ID_Tour = 2,
                Name_Tour = "Nha Trang - Biển xanh cát trắng",
                Description = "Tận hưởng những bãi biển tuyệt đẹp, làn nước trong xanh và các hoạt động vui chơi giải trí hấp dẫn tại Nha Trang.",
                Price = 3990000,
                StartDate = new DateTime(2025, 7, 10),
                EndDate = new DateTime(2025, 7, 13),
                NumberOfPeople = 25,
                NumberOfDays = 3,
                NumberOfNights = 2,
                Type_Tour = "Biển, Lặn biển"
            },
            new Tour
            {
                ID_Tour = 3,
                Name_Tour = "Phong Nha - Kẻ Bàng",
                Description = "Khám phá hệ thống hang động kỳ vĩ, di sản thiên nhiên thế giới tại Vườn quốc gia Phong Nha - Kẻ Bàng.",
                Price = 4790000,
                StartDate = new DateTime(2025, 8, 5),
                EndDate = new DateTime(2025, 8, 9),
                NumberOfPeople = 15,
                NumberOfDays = 3,
                NumberOfNights = 2,
                Type_Tour = "Khám phá, Mạo hiểm"
            },
            new Tour
            {
                ID_Tour = 4,
                Name_Tour = "Hạ Long - Kỳ quan thiên nhiên",
                Description = "Chiêm ngưỡng vẻ đẹp hùng vĩ của Vịnh Hạ Long với hàng ngàn hòn đảo đá vôi lớn nhỏ trên mặt nước xanh biếc.",
                Price = 3690000,
                StartDate = new DateTime(2025, 9, 20),
                EndDate = new DateTime(2025, 9, 22),
                NumberOfPeople = 20,
                NumberOfDays = 3,
                NumberOfNights = 2,
                Type_Tour = "Du thuyền, Tham quan"
            }
        };

        public IActionResult ListTour()
        {
            return View(_mockTours);
        }

        // Sửa action Detail để nhận ID_Tour_Tour và hiển thị chi tiết tour
        public IActionResult Detail(int id)
        {
            // Tìm tour theo ID_Tour trong danh sách mock data
            var tour = _mockTours.FirstOrDefault(t => t.ID_Tour == id);

            if (tour == null)
            {
                // Nếu không tìm thấy tour, có thể trả về trang lỗi 404
                return NotFound();
            }

            return View(tour);
        }
    }
}


    
