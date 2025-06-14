using LAPTRINHWEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TourDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            // 1. Seed Identity data
            await SeedIdentityAsync(roleManager, userManager);

            // 2. Seed business data
            await SeedLocationsAsync(context);
            await SeedAccommodationsAsync(context);
            await SeedTransportationAsync(context);
            await SeedTourGuidesAsync(context);
            await SeedToursAsync(context);
            await SeedTourImagesAsync(context);
            await SeedItinerariesAsync(context);
            await SeedItineraryLocationsAsync(context);
            await SeedTourGuideAssignmentsAsync(context);
            await SeedTourAccommodationsAsync(context);
            await SeedTourTransportsAsync(context);

            await context.SaveChangesAsync();
            Console.WriteLine("✅ Database initialization completed successfully!");
        }

        #region Identity Seeding
        private static async Task SeedIdentityAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[]
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Name = "TourGuide", NormalizedName = "TOURGUIDE" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                    Console.WriteLine($"✅ Created role: {role.Name}");
                }
            }
        }
        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var users = new[]
            {
                new { User = new ApplicationUser
                    {
                        UserName = "admin@easytrips.com",
                        Email = "admin@easytrips.com",
                        Full_Name = "Nguyễn Văn Admin",
                        Phone = "0901000001",
                        Address = "Hà Nội, Việt Nam",
                        Gender = "Nam",
                        EmailConfirmed = true,
                        Created_Date = DateTime.Now
                    },
                    Password = "Admin123!",
                    Role = "Admin"
                },
                new { User = new ApplicationUser
                    {
                        UserName = "manager@easytrips.com",
                        Email = "manager@easytrips.com",
                        Full_Name = "Trần Thị Manager",
                        Phone = "0901000002",
                        Address = "TP.HCM, Việt Nam",
                        Gender = "Nữ",
                        EmailConfirmed = true,
                        Created_Date = DateTime.Now
                    },
                    Password = "Manager123!",
                    Role = "Manager"
                },
                new { User = new ApplicationUser
                    {
                        UserName = "guide@easytrips.com",
                        Email = "guide@easytrips.com",
                        Full_Name = "Lê Văn Hướng dẫn",
                        Phone = "0901000003",
                        Address = "Quảng Ninh, Việt Nam",
                        Gender = "Nam",
                        EmailConfirmed = true,
                        Created_Date = DateTime.Now
                    },
                    Password = "Guide123!",
                    Role = "TourGuide"
                },
                new { User = new ApplicationUser
                    {
                        UserName = "user@easytrips.com",
                        Email = "user@easytrips.com",
                        Full_Name = "Phạm Thị Khách hàng",
                        Phone = "0901000004",
                        Address = "Đà Nẵng, Việt Nam",
                        Gender = "Nữ",
                        EmailConfirmed = true,
                        Created_Date = DateTime.Now
                    },
                    Password = "User123!",
                    Role = "User"
                }
            };

            foreach (var userData in users)
            {
                var existingUser = await userManager.FindByEmailAsync(userData.User.Email);
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(userData.User, userData.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userData.User, userData.Role);
                        Console.WriteLine($"✅ Created {userData.Role}: {userData.User.Email}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Failed to create {userData.Role}: {userData.User.Email}");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"   - {error.Description}");
                        }
                    }
                }
            }
        }
        #endregion

        #region Locations Seeding
        private static async Task SeedLocationsAsync(TourDbContext context)
        {
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location { Name = "Vịnh Hạ Long", Address = "Hạ Long, Quảng Ninh", Description = "Di sản thiên nhiên thế giới UNESCO" },
                    new Location { Name = "Núi Fansipan", Address = "Sapa, Lào Cai", Description = "Nóc nhà Đông Dương" },
                    new Location { Name = "Đền Ngọc Sơn", Address = "Hoàn Kiếm, Hà Nội", Description = "Đền cổ giữa lòng Hồ Gươm" },
                    new Location { Name = "Bản Cát Cát", Address = "Sapa, Lào Cai", Description = "Bản làng người H'Mông" },
                    new Location { Name = "Động Thiên Cung", Address = "Hạ Long, Quảng Ninh", Description = "Hang động đẹp nhất vịnh Hạ Long" },
                    new Location { Name = "Phố cổ Hội An", Address = "Hội An, Quảng Nam", Description = "Di sản văn hóa thế giới UNESCO" },
                    new Location { Name = "Cầu Chùa Cầu", Address = "Hội An, Quảng Nam", Description = "Biểu tượng của phố cổ Hội An" },
                    new Location { Name = "Chùa Cầu Nhật Bản", Address = "Hội An, Quảng Nam", Description = "Kiến trúc độc đáo Nhật - Việt" },
                    new Location { Name = "Bà Nà Hills", Address = "Đà Nẵng", Description = "Khu du lịch trên núi với cầu Vàng nổi tiếng" },
                    new Location { Name = "Đại Nội Huế", Address = "Huế, Thừa Thiên Huế", Description = "Cố đô của triều Nguyễn" },
                    new Location { Name = "Chùa Thiên Mụ", Address = "Huế, Thừa Thiên Huế", Description = "Chùa cổ nhất xứ Huế" },
                    new Location { Name = "Củ Chi Tunnels", Address = "Củ Chi, TP.HCM", Description = "Hệ thống địa đạo lịch sử" },
                    new Location { Name = "Dinh Độc Lập", Address = "Quận 1, TP.HCM", Description = "Dinh thống nhất lịch sử" },
                    new Location { Name = "Chợ Bến Thành", Address = "Quận 1, TP.HCM", Description = "Chợ truyền thống nổi tiếng" },
                    new Location { Name = "Đồng bằng sông Cửu Long", Address = "Cần Thơ", Description = "Miệt vườn sông nước" },
                    new Location { Name = "Chợ nổi Cái Răng", Address = "Cần Thơ", Description = "Chợ nổi đặc trưng miền Tây" },
                    new Location { Name = "Bãi Sao", Address = "Phú Quốc, Kiên Giang", Description = "Bãi biển đẹp nhất Phú Quốc" },
                    new Location { Name = "Cáp treo Hòn Thơm", Address = "Phú Quốc, Kiên Giang", Description = "Cáp treo vượt biển dài nhất thế giới" },
                    new Location { Name = "Thác Elephant", Address = "Đà Lạt, Lâm Đồng", Description = "Thác nước đẹp ở Đà Lạt" },
                    new Location { Name = "Đồi Chè Cầu Đất", Address = "Đà Lạt, Lâm Đồng", Description = "Đồi chè xanh mướt" }
                };
                context.Locations.AddRange(locations);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {locations.Count} locations");
            }
        }
        #endregion

        #region Accommodations Seeding
        private static async Task SeedAccommodationsAsync(TourDbContext context)
        {
            if (!context.Accommodations.Any())
            {
                var accommodations = new List<Accommodation>
                {
                    new Accommodation { Name = "Novotel Hạ Long Bay", Type = (AccommodationType)0, Address = "Hạ Long, Quảng Ninh", Phone = "0203123456", Star_Rating = 5, Description = "Khách sạn 5 sao view vịnh Hạ Long" },
                    new Accommodation { Name = "Du thuyền La Regina", Type = (AccommodationType)2, Address = "Vịnh Hạ Long", Phone = "0203234567", Star_Rating = 4, Description = "Du thuyền cao cấp nghỉ đêm trên vịnh" },
                    new Accommodation { Name = "Sapa Jade Hill Resort", Type = (AccommodationType)1, Address = "Sapa, Lào Cai", Phone = "0214123456", Star_Rating = 4, Description = "Resort view núi Fansipan" },
                    new Accommodation { Name = "Homestay Bản Cát Cát", Type = (AccommodationType)3, Address = "Bản Cát Cát, Sapa", Phone = "0214234567", Star_Rating = 3, Description = "Homestay trải nghiệm văn hóa dân tộc" },
                    new Accommodation { Name = "Anantara Hội An Resort", Type = (AccommodationType)1, Address = "Hội An, Quảng Nam", Phone = "0235123456", Star_Rating = 5, Description = "Resort cao cấp ven sông Thu Bồn" },
                    new Accommodation { Name = "Fusion Maia Đà Nẵng", Type = (AccommodationType)1, Address = "Đà Nẵng", Phone = "0236123456", Star_Rating = 5, Description = "Resort spa all-inclusive" },
                    new Accommodation { Name = "Pilgrimage Village Boutique Resort", Type = (AccommodationType)1, Address = "Huế, Thừa Thiên Huế", Phone = "0234123456", Star_Rating = 4, Description = "Resort kiến trúc làng cổ" },
                    new Accommodation { Name = "Rex Hotel Sài Gòn", Type = (AccommodationType)0, Address = "Quận 1, TP.HCM", Phone = "0283123456", Star_Rating = 4, Description = "Khách sạn lịch sử trung tâm" },
                    new Accommodation { Name = "Park Hyatt Sài Gòn", Type = (AccommodationType)0, Address = "Quận 1, TP.HCM", Phone = "0283234567", Star_Rating = 5, Description = "Khách sạn luxury 5 sao" },
                    new Accommodation { Name = "Victoria Cần Thơ Resort", Type = (AccommodationType)1, Address = "Cần Thơ", Phone = "0292123456", Star_Rating = 4, Description = "Resort ven sông Hậu" },
                    new Accommodation { Name = "JW Marriott Phú Quốc", Type = (AccommodationType)1, Address = "Phú Quốc, Kiên Giang", Phone = "0297123456", Star_Rating = 5, Description = "Resort biển 5 sao" },
                    new Accommodation { Name = "Salinda Resort Phú Quốc", Type = (AccommodationType)1, Address = "Phú Quốc, Kiên Giang", Phone = "0297234567", Star_Rating = 5, Description = "Resort view biển tuyệt đẹp" },
                    new Accommodation { Name = "Ana Mandara Villas Đà Lạt", Type = (AccommodationType)1, Address = "Đà Lạt, Lâm Đồng", Phone = "0263123456", Star_Rating = 5, Description = "Villa resort kiến trúc Pháp cổ" },
                    new Accommodation { Name = "Terracotta Hotel Đà Lạt", Type = (AccommodationType)0, Address = "Đà Lạt, Lâm Đồng", Phone = "0263234567", Star_Rating = 4, Description = "Khách sạn boutique trung tâm" }
                };
                context.Accommodations.AddRange(accommodations);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {accommodations.Count} accommodations");
            }
        }
        #endregion

        #region Transportation Seeding
        private static async Task SeedTransportationAsync(TourDbContext context)
        {
            if (!context.Transportations.Any())
            {
                var transportations = new List<Transportation>
                {
                    new Transportation { Type = 0, Name = "Xe Limousine 16 chỗ VIP", Capacity = 16, Provider = "Hoàng Long Travel", License_Plate = "30A-12345", Description = "Limousine cao cấp ghế massage" },
                    new Transportation { Type = 0, Name = "Xe khách 45 chỗ", Capacity = 45, Provider = "Phương Trang", License_Plate = "51B-67890", Description = "Xe khách đường dài thoải mái" },
                    new Transportation { Type = 0, Name = "Xe 7 chỗ Ford Transit", Capacity = 7, Provider = "Sao Việt Travel", License_Plate = "29A-11111", Description = "Xe gia đình nhỏ gọn" },
                    new Transportation { Type = (TransportationType)1, Name = "Vietnam Airlines A321", Capacity = 180, Provider = "Vietnam Airlines", License_Plate = "VN-A123", Description = "Máy bay Airbus A321" },
                    new Transportation { Type = (TransportationType)1, Name = "Vietjet A320", Capacity = 174, Provider = "Vietjet Air", License_Plate = "VN-A456", Description = "Máy bay Airbus A320" },
                    new Transportation { Type = (TransportationType)1, Name = "Bamboo Airways A321", Capacity = 180, Provider = "Bamboo Airways", License_Plate = "VN-A789", Description = "Máy bay cao cấp" },
                    new Transportation { Type = (TransportationType)2, Name = "Tàu SE1 Hà Nội - Sài Gòn", Capacity = 600, Provider = "Đường sắt Việt Nam", License_Plate = "SE1", Description = "Tàu nhanh Bắc Nam" },
                    new Transportation { Type = (TransportationType)2, Name = "Tàu SE3 Hà Nội - Đà Nẵng", Capacity = 400, Provider = "Đường sắt Việt Nam", License_Plate = "SE3", Description = "Tàu nhanh đi Đà Nẵng" },
                    new Transportation { Type = (TransportationType)3, Name = "Du thuyền Dragon Legend", Capacity = 40, Provider = "Dragon Legend Cruise", License_Plate = "HL-001", Description = "Du thuyền 5 sao vịnh Hạ Long" },
                    new Transportation { Type = (TransportationType)3, Name = "Du thuyền Paradise Peak", Capacity = 30, Provider = "Paradise Cruises", License_Plate = "HL-002", Description = "Du thuyền cao cấp overnight" },
                    new Transportation { Type = (TransportationType)4, Name = "Speedboat SuperDong", Capacity = 300, Provider = "SuperDong", License_Plate = "SD-123", Description = "Tàu cao tốc Rạch Giá - Phú Quốc" },
                    new Transportation { Type = (TransportationType)4, Name = "Cano nhanh 12 chỗ", Capacity = 12, Provider = "Phú Quốc Express", License_Plate = "PQ-456", Description = "Cano tham quan đảo" }
                };
                context.Transportations.AddRange(transportations);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {transportations.Count} transportations");
            }
        }
        #endregion

        #region TourGuides Seeding
        private static async Task SeedTourGuidesAsync(TourDbContext context)
        {
            if (!context.TourGuides.Any())
            {
                var tourGuides = new List<TourGuide>
                {
                    new TourGuide { FullName = "Nguyễn Văn Minh", Phone = "0987654321", Email = "nguyenvanminh@guide.com", Experience = "8 năm hướng dẫn tour miền Bắc, chuyên Hạ Long - Sapa" },
                    new TourGuide { FullName = "Trần Thị Hoa", Phone = "0912345678", Email = "tranthihoa@guide.com", Experience = "6 năm hướng dẫn tour miền Trung, chuyên Hội An - Huế - Đà Nẵng" },
                    new TourGuide { FullName = "Lê Văn Tuấn", Phone = "0923456789", Email = "levantuan@guide.com", Experience = "5 năm hướng dẫn tour miền Nam, chuyên TP.HCM - Mekong Delta" },
                    new TourGuide { FullName = "Phạm Thị Lan", Phone = "0934567890", Email = "phamthilan@guide.com", Experience = "7 năm hướng dẫn tour biển đảo, chuyên Phú Quốc - Nha Trang" },
                    new TourGuide { FullName = "Hoàng Văn Sơn", Phone = "0945678901", Email = "hoangvanson@guide.com", Experience = "4 năm hướng dẫn tour cao nguyên, chuyên Đà Lạt - Sapa" },
                    new TourGuide { FullName = "Ngô Thị Mai", Phone = "0956789012", Email = "ngothimai@guide.com", Experience = "9 năm hướng dẫn tour văn hóa, chuyên Huế - Hội An" },
                    new TourGuide { FullName = "Vũ Văn Hải", Phone = "0967890123", Email = "vuvanhai@guide.com", Experience = "3 năm hướng dẫn tour phiêu lưu, chuyên trekking Sapa" },
                    new TourGuide { FullName = "Đỗ Thị Linh", Phone = "0978901234", Email = "dothilinh@guide.com", Experience = "5 năm hướng dẫn tour ẩm thực, chuyên tour ẩm thực TP.HCM" }
                };
                context.TourGuides.AddRange(tourGuides);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {tourGuides.Count} tour guides");
            }
        }
        #endregion

        #region Tours Seeding
        private static async Task SeedToursAsync(TourDbContext context)
        {
            if (!context.Tours.Any())
            {
                var tours = new List<Tour>
                {
                    new Tour
                    {
                        Name_Tour = "Khám phá Vịnh Hạ Long - Đảo Cát Bà",
                        Description = "Tour khám phá vịnh Hạ Long 3 ngày 2 đêm với du thuyền 5 sao, tham quan động Thiên Cung, đảo Cát Bà, thưởng thức hải sản tươi ngon và trải nghiệm kayak.",
                        Duration = 3,
                        Start_Location = "Hà Nội",
                        End_Location = "Hạ Long",
                        Price = 4500000,
                        Discount = 500000,
                        Max_Capacity = 30,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Sapa - Fansipan - Bản Cát Cát",
                        Description = "Tour trekking Sapa 4 ngày 3 đêm chinh phục đỉnh Fansipan, khám phá văn hóa dân tộc H'Mông, thưởng thức ẩm thực đặc sản vùng cao và ngủ homestay.",
                        Duration = 4,
                        Start_Location = "Hà Nội",
                        End_Location = "Sapa",
                        Price = 5200000,
                        Discount = 700000,
                        Max_Capacity = 20,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Hội An - Đà Nẵng - Bà Nà Hills",
                        Description = "Tour khám phá di sản văn hóa Hội An 3 ngày 2 đêm, tham quan phố cổ, chùa Cầu, làng nghề truyền thống, lên Bà Nà Hills trải nghiệm cầu Vàng và Fantasy Park.",
                        Duration = 3,
                        Start_Location = "Đà Nẵng",
                        End_Location = "Hội An",
                        Price = 3800000,
                        Discount = 300000,
                        Max_Capacity = 25,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Huế - Cố đô Hoàng gia",
                        Description = "Tour văn hóa lịch sử Huế 2 ngày 1 đêm, tham quan Đại Nội, Lăng Khải Định, chùa Thiên Mụ, thưởng thức bún bò Huế và nghe Ca Huế cung đình.",
                        Duration = 2,
                        Start_Location = "Đà Nẵng",
                        End_Location = "Huế",
                        Price = 2800000,
                        Discount = 0,
                        Max_Capacity = 35,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "TP.HCM - Củ Chi - Mekong Delta",
                        Description = "Tour khám phá Sài Gòn và đồng bằng sông Cửu Long 3 ngày 2 đêm, tham quan địa đạo Củ Chi, chợ nổi Cái Răng, làng nghề kẹo dừa, trải nghiệm đi xuồng trong rừng tràm.",
                        Duration = 3,
                        Start_Location = "TP.HCM",
                        End_Location = "Cần Thơ",
                        Price = 3200000,
                        Discount = 200000,
                        Max_Capacity = 40,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Phú Quốc - Đảo Ngọc",
                        Description = "Tour nghỉ dưỡng Phú Quốc 4 ngày 3 đêm, tắm biển bãi Sao, cáp treo Hòn Thơm, Safari Phú Quốc, Grand World, chợ đêm Dinh Cậu và thưởng thức hải sản.",
                        Duration = 4,
                        Start_Location = "TP.HCM",
                        End_Location = "Phú Quốc",
                        Price = 6800000,
                        Discount = 800000,
                        Max_Capacity = 30,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Đà Lạt - Thành phố Ngàn hoa",
                        Description = "Tour Đà Lạt 3 ngày 2 đêm, tham quan thác Elephant, đồi chè Cầu Đất, ga Đà Lạt, chợ đêm, thuê xe máy dạo phố, thưởng thức bánh tráng nướng và sữa đậu nành.",
                        Duration = 3,
                        Start_Location = "TP.HCM",
                        End_Location = "Đà Lạt",
                        Price = 4200000,
                        Discount = 400000,
                        Max_Capacity = 25,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Hà Nội - Thủ đô Ngàn năm",
                        Description = "Tour khám phá Hà Nội 2 ngày 1 đêm, tham quan Văn Miếu, Lăng Bác, phố cổ, hồ Gươm, thưởng thức phở, bún chả và cà phê vỉa hè.",
                        Duration = 2,
                        Start_Location = "Hà Nội",
                        End_Location = "Hà Nội",
                        Price = 2200000,
                        Discount = 0,
                        Max_Capacity = 30,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Nha Trang - Biển xanh cát trắng",
                        Description = "Tour biển Nha Trang 4 ngày 3 đêm, tắm biển, lặn san hô, tham quan Vinpearl Land, tháp Bà Ponagar và tắm bùn khoáng nóng.",
                        Duration = 4,
                        Start_Location = "TP.HCM",
                        End_Location = "Nha Trang",
                        Price = 5500000,
                        Discount = 600000,
                        Max_Capacity = 35,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Tour Miền Bắc Trọn gói 7 ngày",
                        Description = "Tour tổng hợp miền Bắc 7 ngày 6 đêm: Hà Nội - Hạ Long - Sapa - Ninh Bình, trải nghiệm văn hóa, thiên nhiên và ẩm thực miền Bắc.",
                        Duration = 7,
                        Start_Location = "Hà Nội",
                        End_Location = "Hà Nội",
                        Price = 9800000,
                        Discount = 1200000,
                        Max_Capacity = 25,
                        Status = TourStatus.Active
                    }
                };
                context.Tours.AddRange(tours);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {tours.Count} tours");
            }
        }
        #endregion

        #region Tour Images Seeding
        private static async Task SeedTourImagesAsync(TourDbContext context)
        {
            if (!context.TourImages.Any())
            {
                var tours = await context.Tours.ToListAsync();
                var images = new List<TourImage>();
                foreach (var tour in tours)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        images.Add(new TourImage
                        {
                            ID_Tour = tour.ID_Tour,
                            Image_URL = $"/img/tours/tour_{tour.ID_Tour}_{i}.jpg",
                            Caption = $"Hình {i} - {tour.Name_Tour}"
                        });
                    }
                }
                context.TourImages.AddRange(images);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {images.Count} tour images");
            }
        }
        #endregion

        #region Itineraries and ItineraryDetails Seeding
        // Ở đây ta tạo lịch trình cơ bản (Itinerary) và kèm theo mỗi lịch trình là các chi tiết (ItineraryDetail)
        private static async Task SeedItinerariesAsync(TourDbContext context)
        {
            if (!context.Itineraries.Any())
            {
                var tours = await context.Tours.ToListAsync();
                // Danh sách tạm để thêm cả Itinerary và ItineraryDetail thông qua navgiation property
                var itineraries = new List<Itinerary>();

                foreach (var tour in tours)
                {
                    // Ví dụ: áp dụng seeding cho một số tour nổi bật
                    switch (tour.Name_Tour)
                    {
                        case "Khám phá Vịnh Hạ Long - Đảo Cát Bà":
                            {
                                // Day 1
                                var iti1 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 1,
                                    Title = "Hà Nội - Hạ Long",
                                    Details = new List<ItineraryDetail>()
                                };
                                iti1.Details.Add(new ItineraryDetail { Time = "06:00", Activities = "Xe đón tại Hà Nội" });
                                iti1.Details.Add(new ItineraryDetail { Time = "08:30", Activities = "Nghỉ chân Hải Dương" });
                                iti1.Details.Add(new ItineraryDetail { Time = "12:00", Activities = "Đến Hạ Long, lên du thuyền" });
                                iti1.Details.Add(new ItineraryDetail { Time = "13:00", Activities = "Ăn trưa trên du thuyền" });
                                iti1.Details.Add(new ItineraryDetail { Time = "15:00", Activities = "Tham quan động Thiên Cung" });
                                iti1.Details.Add(new ItineraryDetail { Time = "18:00", Activities = "Ăn tối và nghỉ đêm trên du thuyền" });

                                // Day 2
                                var iti2 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 2,
                                    Title = "Hạ Long - Đảo Cát Bà",
                                    Details = new List<ItineraryDetail>()
                                };
                                iti2.Details.Add(new ItineraryDetail { Time = "07:00", Activities = "Tập thể dục và ăn sáng" });
                                iti2.Details.Add(new ItineraryDetail { Time = "09:00", Activities = "Kayak khám phá hang động" });
                                iti2.Details.Add(new ItineraryDetail { Time = "11:30", Activities = "Di chuyển đến đảo Cát Bà" });
                                iti2.Details.Add(new ItineraryDetail { Time = "13:00", Activities = "Ăn trưa và nghỉ ngơi" });
                                iti2.Details.Add(new ItineraryDetail { Time = "15:00", Activities = "Tham quan Vườn quốc gia Cát Bà" });
                                iti2.Details.Add(new ItineraryDetail { Time = "19:00", Activities = "Ăn tối hải sản và tự do khám phá" });

                                // Day 3
                                var iti3 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 3,
                                    Title = "Cát Bà - Hà Nội",
                                    Details = new List<ItineraryDetail>()
                                };
                                iti3.Details.Add(new ItineraryDetail { Time = "08:00", Activities = "Ăn sáng và tham quan bãi biển" });
                                iti3.Details.Add(new ItineraryDetail { Time = "10:00", Activities = "Mua sắm đặc sản" });
                                iti3.Details.Add(new ItineraryDetail { Time = "12:00", Activities = "Ăn trưa và về Hà Nội" });
                                iti3.Details.Add(new ItineraryDetail { Time = "18:00", Activities = "Kết thúc tour" });

                                itineraries.AddRange(new[] { iti1, iti2, iti3 });
                            }
                            break;

                        case "Sapa - Fansipan - Bản Cát Cát":
                            {
                                // Day 1
                                var iti1 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 1,
                                    Title = "Hà Nội - Sapa"
                                };
                                iti1.Details.Add(new ItineraryDetail { Time = "21:30", Activities = "Xe giường nằm khởi hành từ Hà Nội" });

                                // Day 2
                                var iti2 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 2,
                                    Title = "Chinh phục Fansipan"
                                };
                                iti2.Details.Add(new ItineraryDetail { Time = "06:00", Activities = "Đến Sapa và ăn sáng" });
                                iti2.Details.Add(new ItineraryDetail { Time = "08:00", Activities = "Cáp treo lên đỉnh Fansipan" });
                                iti2.Details.Add(new ItineraryDetail { Time = "11:00", Activities = "Chụp ảnh tại đỉnh" });
                                iti2.Details.Add(new ItineraryDetail { Time = "13:00", Activities = "Ăn trưa & nghỉ ngơi" });
                                iti2.Details.Add(new ItineraryDetail { Time = "15:00", Activities = "Tham quan bản Cát Cát" });
                                iti2.Details.Add(new ItineraryDetail { Time = "19:00", Activities = "Ăn tối và nghỉ đêm khách sạn" });

                                // Day 3
                                var iti3 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 3,
                                    Title = "Sapa - Làng dân tộc"
                                };
                                iti3.Details.Add(new ItineraryDetail { Time = "08:00", Activities = "Ăn sáng và trekking tại bản Tả Van" });
                                iti3.Details.Add(new ItineraryDetail { Time = "12:00", Activities = "Ăn trưa với gia đình H'Mông" });
                                iti3.Details.Add(new ItineraryDetail { Time = "14:00", Activities = "Trải nghiệm làm bánh giầy" });
                                iti3.Details.Add(new ItineraryDetail { Time = "16:00", Activities = "Mua sắm thổ cẩm" });
                                iti3.Details.Add(new ItineraryDetail { Time = "19:00", Activities = "BBQ tối và nghỉ homestay" });

                                // Day 4
                                var iti4 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 4,
                                    Title = "Sapa - Hà Nội"
                                };
                                iti4.Details.Add(new ItineraryDetail { Time = "08:00", Activities = "Ăn sáng" });
                                iti4.Details.Add(new ItineraryDetail { Time = "10:00", Activities = "Khởi hành về Hà Nội" });
                                iti4.Details.Add(new ItineraryDetail { Time = "16:00", Activities = "Về đến Hà Nội, kết thúc tour" });

                                itineraries.AddRange(new[] { iti1, iti2, iti3, iti4 });
                            }
                            break;

                        case "Hội An - Đà Nẵng - Bà Nà Hills":
                            {
                                // Day 1
                                var iti1 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 1,
                                    Title = "Đến Đà Nẵng - Hội An"
                                };
                                iti1.Details.Add(new ItineraryDetail { Time = "10:00", Activities = "Đón sân bay Đà Nẵng" });
                                iti1.Details.Add(new ItineraryDetail { Time = "11:00", Activities = "Check-in khách sạn" });
                                iti1.Details.Add(new ItineraryDetail { Time = "13:00", Activities = "Ăn trưa cao lầu Hội An" });
                                iti1.Details.Add(new ItineraryDetail { Time = "15:00", Activities = "Tham quan phố cổ và chùa Cầu" });
                                iti1.Details.Add(new ItineraryDetail { Time = "18:00", Activities = "Ăn tối white rose" });
                                iti1.Details.Add(new ItineraryDetail { Time = "20:00", Activities = "Thả đèn hoa đăng sông Hoài" });

                                // Day 2
                                var iti2 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 2,
                                    Title = "Bà Nà Hills"
                                };
                                iti2.Details.Add(new ItineraryDetail { Time = "07:30", Activities = "Ăn sáng và đi Bà Nà Hills" });
                                iti2.Details.Add(new ItineraryDetail { Time = "08:30", Activities = "Cáp treo lên Bà Nà" });
                                iti2.Details.Add(new ItineraryDetail { Time = "10:00", Activities = "Tham quan Cầu Vàng và chụp ảnh" });
                                iti2.Details.Add(new ItineraryDetail { Time = "12:00", Activities = "Ăn trưa buffet" });
                                iti2.Details.Add(new ItineraryDetail { Time = "14:00", Activities = "Tham gia Fantasy Park" });
                                iti2.Details.Add(new ItineraryDetail { Time = "17:00", Activities = "Về Đà Nẵng và ăn tối hải sản" });

                                // Day 3
                                var iti3 = new Itinerary
                                {
                                    ID_Tour = tour.ID_Tour,
                                    Day_Number = 3,
                                    Title = "Đà Nẵng - Về"
                                };
                                iti3.Details.Add(new ItineraryDetail { Time = "08:00", Activities = "Ăn sáng và tắm biển Mỹ Khê" });
                                iti3.Details.Add(new ItineraryDetail { Time = "10:00", Activities = "Mua sắm tại Con Market" });
                                iti3.Details.Add(new ItineraryDetail { Time = "12:00", Activities = "Ăn trưa và ra sân bay" });
                                iti3.Details.Add(new ItineraryDetail { Time = "14:00", Activities = "Bay về, kết thúc tour" });

                                itineraries.AddRange(new[] { iti1, iti2, iti3 });
                            }
                            break;

                        default:
                            {
                                // Với các tour khác, tạo một bản ghi chi tiết đơn giản cho mỗi ngày
                                for (int day = 1; day <= tour.Duration; day++)
                                {
                                    var iti = new Itinerary
                                    {
                                        ID_Tour = tour.ID_Tour,
                                        Day_Number = day,
                                        Title = $"Ngày {day}"
                                    };
                                    iti.Details.Add(new ItineraryDetail
                                    {
                                        Time = $"Thời gian hoạt động ngày {day}",
                                        Activities = $"Hoạt động cụ thể ngày {day} của tour {tour.Name_Tour}"
                                    });
                                    itineraries.Add(iti);
                                }
                            }
                            break;
                    }
                }
                context.Itineraries.AddRange(itineraries);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {itineraries.Count} itineraries with details");

                // Seed ItineraryLocations nếu cần
                await SeedItineraryLocationsAsync(context);
            }
        }
        #endregion

        #region ItineraryLocations Seeding
        private static async Task SeedItineraryLocationsAsync(TourDbContext context)
        {
            if (!context.ItineraryLocations.Any())
            {
                var itineraries = await context.Itineraries.ToListAsync();
                var locations = await context.Locations.ToListAsync();
                var itineraryLocations = new List<ItineraryLocation>();
                foreach (var itinerary in itineraries.Take(10))
                {
                    var randomLocations = locations.OrderBy(x => Guid.NewGuid()).Take(2);
                    foreach (var location in randomLocations)
                    {
                        itineraryLocations.Add(new ItineraryLocation
                        {
                            ID_Itinerary = itinerary.ID_Itinerary,
                            ID_Location = location.ID_Location,
                        });
                    }
                }
                context.ItineraryLocations.AddRange(itineraryLocations);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {itineraryLocations.Count} itinerary locations");
            }
        }
        #endregion

        #region Tour Assignments Seeding
        private static async Task SeedTourGuideAssignmentsAsync(TourDbContext context)
        {
            if (!context.TourGuideAssignments.Any())
            {
                var tours = await context.Tours.ToListAsync();
                var guides = await context.TourGuides.ToListAsync();
                var assignments = new List<TourGuideAssignment>();
                for (int i = 0; i < tours.Count; i++)
                {
                    var tour = tours[i];
                    var guide = guides[i % guides.Count];
                    assignments.Add(new TourGuideAssignment
                    {
                        ID_Tour = tour.ID_Tour,
                        ID_Guide = guide.ID_Guide,
                        Start_Date = DateTime.Now.AddDays(7 + i * 3),
                        End_Date = DateTime.Now.AddDays(7 + i * 3 + tour.Duration)
                    });
                }
                context.TourGuideAssignments.AddRange(assignments);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {assignments.Count} tour guide assignments");
            }
        }
        private static async Task SeedTourAccommodationsAsync(TourDbContext context)
        {
            if (!context.TourAccommodations.Any())
            {
                var tours = await context.Tours.ToListAsync();
                var accommodations = await context.Accommodations.ToListAsync();
                var tourAccommodations = new List<TourAccommodation>();
                foreach (var tour in tours)
                {
                    for (int night = 1; night < tour.Duration; night++)
                    {
                        var accommodation = accommodations[night % accommodations.Count];
                        tourAccommodations.Add(new TourAccommodation
                        {
                            ID_Tour = tour.ID_Tour,
                            ID_Accommodation = accommodation.ID_Accommodation,
                            Day_Number = night,
                            Nights = 1,
                            Checkin_Time = new TimeSpan(14, 0, 0),
                            Checkout_Time = new TimeSpan(12, 0, 0)
                        });
                    }
                }
                context.TourAccommodations.AddRange(tourAccommodations);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {tourAccommodations.Count} tour accommodations");
            }
        }
        private static async Task SeedTourTransportsAsync(TourDbContext context)
        {
            if (!context.TourTransports.Any())
            {
                var tours = await context.Tours.ToListAsync();
                var transportations = await context.Transportations.ToListAsync();
                var tourTransports = new List<TourTransport>();
                foreach (var tour in tours)
                {
                    var transport = transportations.First(t => t.Type == 0);
                    tourTransports.Add(new TourTransport
                    {
                        ID_Tour = tour.ID_Tour,
                        ID_Transport = transport.ID_Transport,
                        Day_Number = 1,
                        From_Location = tour.Start_Location,
                        To_Location = tour.End_Location,
                        Departure_Time = new TimeSpan(7, 0, 0),
                        Arrival_Time = new TimeSpan(12, 0, 0)
                    });
                    if (tour.Duration > 1)
                    {
                        tourTransports.Add(new TourTransport
                        {
                            ID_Tour = tour.ID_Tour,
                            ID_Transport = transport.ID_Transport,
                            Day_Number = tour.Duration,
                            From_Location = tour.End_Location,
                            To_Location = tour.Start_Location,
                            Departure_Time = new TimeSpan(14, 0, 0),
                            Arrival_Time = new TimeSpan(18, 0, 0)
                        });
                    }
                }
                context.TourTransports.AddRange(tourTransports);
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Seeded {tourTransports.Count} tour transports");
            }
        }
        #endregion




    }
}