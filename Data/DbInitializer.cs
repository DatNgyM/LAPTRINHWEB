using LAPTRINHWEB.Models;
using LAPTRINHWEB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

            await context.Database.EnsureCreatedAsync();

            await SeedRolesAsync(context);
            await SeedUsersAsync(context, authService);
            await SeedLocationsAsync(context);
            await SeedToursAsync(context);
            await SeedTourGuidesAsync(context);
            await SeedAccommodationsAsync(context);
            await SeedTransportationAsync(context);
            await SeedTourGuideAssignmentsAsync(context);
            await SeedTourAccommodationsAsync(context);
            await SeedTourTransportsAsync(context);


            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(TourDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Role_Name = "Admin", Description = "Quản trị viên", Created_Date = DateTime.Now, Is_Active = true },
                    new Role { Role_Name = "Manager", Description = "Quản lý tour", Created_Date = DateTime.Now, Is_Active = true },
                    new Role { Role_Name = "TourGuide", Description = "Hướng dẫn viên", Created_Date = DateTime.Now, Is_Active = true },
                    new Role { Role_Name = "Customer", Description = "Khách hàng", Created_Date = DateTime.Now, Is_Active = true }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUsersAsync(TourDbContext context, IAuthService authService)
        {
            if (!context.Users.Any())
            {
                var adminRole = context.Roles.First(r => r.Role_Name == "Admin");
                var managerRole = context.Roles.First(r => r.Role_Name == "Manager");
                var guideRole = context.Roles.First(r => r.Role_Name == "TourGuide");
                var customerRole = context.Roles.First(r => r.Role_Name == "Customer");

                var users = new List<User>
                {
                    new User
                    {
                        Full_Name = "Quản trị viên",
                        Email = "admin@easytrips.com",
                        Password_Hash = authService.HashPassword("Admin@123"),
                        Phone = "0123456789",
                        Address = "Hà Nội",
                        Gender = "Nam",
                        ID_Role = adminRole.ID_Role,
                        Created_Date = DateTime.Now,
                        Is_Active = true,
                        Email_Verified = true
                    },
                    new User
                    {
                        Full_Name = "Nguyễn Văn Quản",
                        Email = "manager@easytrips.com",
                        Password_Hash = authService.HashPassword("Manager@123"),
                        Phone = "0987654321",
                        Address = "TP.HCM",
                        Gender = "Nam",
                        ID_Role = managerRole.ID_Role,
                        Created_Date = DateTime.Now,
                        Is_Active = true,
                        Email_Verified = true
                    },
                    new User
                    {
                        Full_Name = "Trần Thị Lan",
                        Email = "guide@easytrips.com",
                        Password_Hash = authService.HashPassword("Guide@123"),
                        Phone = "0976543210",
                        Address = "Đà Nẵng",
                        Gender = "Nữ",
                        Date_Of_Birth = new DateTime(1990, 5, 15),
                        ID_Role = guideRole.ID_Role,
                        Created_Date = DateTime.Now,
                        Is_Active = true,
                        Email_Verified = true
                    },
                    new User
                    {
                        Full_Name = "Lê Thị Hoa",
                        Email = "customer@easytrips.com",
                        Password_Hash = authService.HashPassword("Customer@123"),
                        Phone = "0901234567",
                        Address = "Hà Nội",
                        Gender = "Nữ",
                        Date_Of_Birth = new DateTime(1985, 8, 20),
                        ID_Role = customerRole.ID_Role,
                        Created_Date = DateTime.Now,
                        Is_Active = true,
                        Email_Verified = true
                    }
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedLocationsAsync(TourDbContext context)
        {
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location { Name = "Vịnh Hạ Long", Address = "Quảng Ninh", Description = "Di sản thiên nhiên thế giới" },
                    new Location { Name = "Phố cổ Hội An", Address = "Quảng Nam", Description = "Di sản văn hóa thế giới" },
                    new Location { Name = "Sapa", Address = "Lào Cai", Description = "Thành phố trong sương" }
                };
                context.Locations.AddRange(locations);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedToursAsync(TourDbContext context)
        {
            if (!context.Tours.Any())
            {
                var tours = new List<Tour>
                {
                    new Tour
                    {
                        Name_Tour = "Khám phá vịnh Hạ Long",
                        Description = "Tour du lịch vịnh Hạ Long 2 ngày 1 đêm",
                        Duration = 2,
                        Start_Location = "Hà Nội",
                        End_Location = "Hạ Long",
                        Price = 2500000,
                        Discount = 0,
                        Max_Capacity = 25,
                        Status = TourStatus.Active
                    },
                    new Tour
                    {
                        Name_Tour = "Hội An - Đà Nẵng",
                        Description = "Khám phá phố cổ Hội An và Đà Nẵng",
                        Duration = 3,
                        Start_Location = "Đà Nẵng",
                        End_Location = "Hội An",
                        Price = 4200000,
                        Discount = 200000,
                        Max_Capacity = 20,
                        Status = TourStatus.Active
                    }
                };
                context.Tours.AddRange(tours);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTourGuidesAsync(TourDbContext context)
        {
            if (!context.TourGuides.Any())
            {
                var guides = new List<TourGuide>
                {
                    new TourGuide
                    {
                        FullName = "Nguyễn Văn Minh",
                        Phone = "0987654321",
                        Email = "nguyenvanminh@email.com",
                        Experience = "5 năm hướng dẫn tour miền Bắc"
                    }
                };
                context.TourGuides.AddRange(guides);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAccommodationsAsync(TourDbContext context)
        {
            if (!context.Accommodations.Any())
            {
                var accommodations = new List<Accommodation>
                {
                    new Accommodation
                    {
                        Name = "Khách sạn Hạ Long Bay",
                        Type = 0,
                        Address = "Bãi Cháy, Hạ Long",
                        Phone = "0203123456",
                        Star_Rating = 4,
                        Description = "Khách sạn 4 sao view vịnh Hạ Long"
                    }
                };
                context.Accommodations.AddRange(accommodations);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTransportationAsync(TourDbContext context)
        {
            if (!context.Transportations.Any())
            {
                var transportations = new List<Transportation>
                {
                    new Transportation
                    {
                        Type = 0,
                        Name = "Xe Limousine Hoàng Long",
                        Capacity = 16,
                        Provider = "Hoàng Long",
                        License_Plate = "30A-12345",
                        Description = "Xe limousine 16 chỗ cao cấp"
                    }
                };
                context.Transportations.AddRange(transportations);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTourGuideAssignmentsAsync(TourDbContext context)
        {
            if (!context.TourGuideAssignments.Any())
            {
                var tour = context.Tours.FirstOrDefault();
                var guide = context.TourGuides.FirstOrDefault();
                if (tour != null && guide != null)
                {
                    var assignment = new TourGuideAssignment
                    {
                        ID_Tour = tour.ID_Tour,
                        ID_Guide = guide.ID_Guide,
                        Start_Date = DateTime.Now.AddDays(7),
                        End_Date = DateTime.Now.AddDays(8)
                    };
                    context.TourGuideAssignments.Add(assignment);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedTourAccommodationsAsync(TourDbContext context)
        {
            if (!context.TourAccommodations.Any())
            {
                var tour = context.Tours.FirstOrDefault();
                var acc = context.Accommodations.FirstOrDefault();
                if (tour != null && acc != null)
                {
                    var tourAcc = new TourAccommodation
                    {
                        ID_Tour = tour.ID_Tour,
                        ID_Accommodation = acc.ID_Accommodation,
                        Day_Number = 1,
                        Nights = 1,
                        Checkin_Time = new TimeSpan(14, 0, 0),
                        Checkout_Time = new TimeSpan(12, 0, 0)
                    };
                    context.TourAccommodations.Add(tourAcc);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedTourTransportsAsync(TourDbContext context)
        {
            if (!context.TourTransports.Any())
            {
                var tour = context.Tours.FirstOrDefault();
                var transport = context.Transportations.FirstOrDefault();
                if (tour != null && transport != null)
                {
                    var tourTransport = new TourTransport
                    {
                        ID_Tour = tour.ID_Tour,
                        ID_Transport = transport.ID_Transport,
                        Day_Number = 1,
                        From_Location = tour.Start_Location,
                        To_Location = tour.End_Location,
                        Departure_Time = new TimeSpan(7, 0, 0),
                        Arrival_Time = new TimeSpan(10, 0, 0)
                    };
                    context.TourTransports.Add(tourTransport);
                    await context.SaveChangesAsync();
                }
            }
        }




    }
}