using LAPTRINHWEB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(TourDbContext context)
        {
            // Đảm bảo database đã được tạo
            context.Database.EnsureCreated();

            // Seed Locations (Địa điểm du lịch Việt Nam)
            await SeedLocationsAsync(context);

            // Seed Tours (Tour trong nước)
            await SeedToursAsync(context);

            // Seed Accommodations (Khách sạn)
            await SeedAccommodationsAsync(context);

            // Seed Transportation (Phương tiện)
            await SeedTransportationAsync(context);

            // Seed TourGuides (Hướng dẫn viên)
            await SeedTourGuidesAsync(context);

            // Seed Customers (Khách hàng mẫu)
            await SeedCustomersAsync(context);

            // Seed Itineraries (Lịch trình tour)
            await SeedItinerariesAsync(context);

            // Seed TourImages (Hình ảnh tour)
            await SeedTourImagesAsync(context);

            // Seed TourAccommodations (Liên kết tour với khách sạn)
            await SeedTourAccommodationsAsync(context);

            // Seed TourTransports (Liên kết tour với phương tiện)
            await SeedTourTransportsAsync(context);

            // Seed TourGuideAssignments (Phân công hướng dẫn viên)
            await SeedTourGuideAssignmentsAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedLocationsAsync(TourDbContext context)
        {
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location {
                        Name = "Vịnh Hạ Long",
                        Description = "Vịnh Hạ Long - Di sản thiên nhiên thế giới với hàng nghìn đảo đá vôi",
                        Address = "Thành phố Hạ Long, Quảng Ninh, Việt Nam"
                    },
                    new Location {
                        Name = "Phố cổ Hội An",
                        Description = "Phố cổ Hội An - Di sản văn hóa thế giới với kiến trúc cổ kính",
                        Address = "Hội An, Quảng Nam, Việt Nam"
                    },
                    new Location {
                        Name = "Sapa",
                        Description = "Thành phố trong sương với ruộng bậc thang tuyệt đẹp",
                        Address = "Thị trấn Sapa, Lào Cai, Việt Nam"
                    },
                    new Location {
                        Name = "Đà Lạt",
                        Description = "Thành phố ngàn hoa với khí hậu mát mẻ quanh năm",
                        Address = "Thành phố Đà Lạt, Lâm Đồng, Việt Nam"
                    },
                    new Location {
                        Name = "Mũi Né",
                        Description = "Bãi biển đẹp với đồi cát trắng và đỏ nổi tiếng",
                        Address = "Mũi Né, Phan Thiết, Bình Thuận, Việt Nam"
                    },
                    new Location {
                        Name = "Phú Quốc",
                        Description = "Đảo ngọc với bãi biển trong xanh và hải sản tươi ngon",
                        Address = "Đảo Phú Quốc, Kiên Giang, Việt Nam"
                    },
                    new Location {
                        Name = "Ninh Bình",
                        Description = "Tràng An - Tam Cốc với cảnh quan núi nước hữu tình",
                        Address = "Thành phố Ninh Bình, Ninh Bình, Việt Nam"
                    },
                    new Location {
                        Name = "Đà Nẵng",
                        Description = "Thành phố đáng sống với nhiều bãi biển đẹp",
                        Address = "Thành phố Đà Nẵng, Việt Nam"
                    }
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
                    new Tour {
                        Name_Tour = "Khám phá vịnh Hạ Long 2N1Đ",
                        Description = "Tour du lịch vịnh Hạ Long với du thuyền sang trọng, tham quan hang Sửng Sốt, đảo Titop",
                        Duration = 2,
                        Start_Location = "Hà Nội",
                        End_Location = "Hạ Long",
                        Price = 2500000,
                        Discount = 0,
                        Max_Capacity = 25,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Hội An - Đà Nẵng 3N2Đ",
                        Description = "Khám phá phố cổ Hội An, Bà Nà Hills, cầu Vàng và các bãi biển đẹp Đà Nẵng",
                        Duration = 3,
                        Start_Location = "Đà Nẵng",
                        End_Location = "Hội An",
                        Price = 4200000,
                        Discount = 200000,
                        Max_Capacity = 20,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Sapa - Fansipan 3N2Đ",
                        Description = "Chinh phục đỉnh Fansipan, thăm bản Cát Cát, chợ tình Sapa",
                        Duration = 3,
                        Start_Location = "Hà Nội",
                        End_Location = "Sapa",
                        Price = 3800000,
                        Discount = 100000,
                        Max_Capacity = 15,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Đà Lạt thành phố ngàn hoa 3N2Đ",
                        Description = "Thăm thác Elephant, thiền viện Trúc Lâm, đồi chè Cầu Đất, chợ đêm Đà Lạt",
                        Duration = 3,
                        Start_Location = "TP.HCM",
                        End_Location = "Đà Lạt",
                        Price = 3500000,
                        Discount = 0,
                        Max_Capacity = 18,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Mũi Né - Đồi cát bay 2N1Đ",
                        Description = "Trải nghiệm đồi cát trắng, đồi cát đỏ, suối tiên, làng chài Mũi Né",
                        Duration = 2,
                        Start_Location = "TP.HCM",
                        End_Location = "Mũi Né",
                        Price = 2200000,
                        Discount = 0,
                        Max_Capacity = 22,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Phú Quốc đảo ngọc 4N3Đ",
                        Description = "Khám phá đảo Phú Quốc: cáp treo Hòn Thơm, Grand World, Safari, chợ đêm",
                        Duration = 4,
                        Start_Location = "TP.HCM",
                        End_Location = "Phú Quốc",
                        Price = 6800000,
                        Discount = 500000,
                        Max_Capacity = 16,
                        Status = TourStatus.Active
                    },
                    new Tour {
                        Name_Tour = "Ninh Bình - Tràng An 2N1Đ",
                        Description = "Du ngoạn Tràng An, Tam Cốc, chùa Bái Đính, hang Múa",
                        Duration = 2,
                        Start_Location = "Hà Nội",
                        End_Location = "Ninh Bình",
                        Price = 2800000,
                        Discount = 0,
                        Max_Capacity = 20,
                        Status = TourStatus.Active
                    }
                };

                context.Tours.AddRange(tours);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAccommodationsAsync(TourDbContext context)
        {
            if (!context.Accommodations.Any())
            {
                var accommodations = new List<Accommodation>
                {
                    new Accommodation {
                        Name = "Khách sạn Hạ Long Bay",
                        Type = AccommodationType.Hotel,
                        Address = "Bãi Cháy, Hạ Long, Quảng Ninh",
                        Phone = "0203123456",
                        Star_Rating = 4,
                        Description = "Khách sạn 4 sao view vịnh Hạ Long tuyệt đẹp"
                    },
                    new Accommodation {
                        Name = "Resort Hội An Beach",
                        Type = AccommodationType.Resort,
                        Address = "Cửa Đại, Hội An, Quảng Nam",
                        Phone = "0235123456",
                        Star_Rating = 5,
                        Description = "Resort 5 sao bên bờ biển Cửa Đại"
                    },
                    new Accommodation {
                        Name = "Hotel Sapa Valley",
                        Type = AccommodationType.Hotel,
                        Address = "Trung tâm thị trấn Sapa, Lào Cai",
                        Phone = "0214123456",
                        Star_Rating = 3,
                        Description = "Khách sạn view thung lũng Sapa"
                    },
                    new Accommodation {
                        Name = "Dalat Palace Hotel",
                        Type = AccommodationType.Hotel,
                        Address = "Trung tâm thành phố Đà Lạt",
                        Phone = "0263123456",
                        Star_Rating = 4,
                        Description = "Khách sạn phong cách Pháp cổ kính"
                    },
                    new Accommodation {
                        Name = "Mũi Né Resort",
                        Type = AccommodationType.Resort,
                        Address = "Bãi biển Mũi Né, Phan Thiết",
                        Phone = "0252123456",
                        Star_Rating = 4,
                        Description = "Resort view biển Mũi Né"
                    },
                    new Accommodation {
                        Name = "Phú Quốc Island Resort",
                        Type = AccommodationType.Resort,
                        Address = "Bãi Trường, Phú Quốc, Kiên Giang",
                        Phone = "0297123456",
                        Star_Rating = 5,
                        Description = "Resort 5 sao trên bãi biển đẹp nhất Phú Quốc"
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
                    new Transportation {
                        Type = TransportationType.Bus,
                        Name = "Xe limousine 16 chỗ",
                        Capacity = 16,
                        Provider = "Hoàng Long Limousine",
                        License_Plate = "30A-12345",
                        Description = "Xe limousine 16 chỗ cao cấp có WiFi, nước uống"
                    },
                    new Transportation {
                        Type = TransportationType.Bus,
                        Name = "Xe khách 45 chỗ",
                        Capacity = 45,
                        Provider = "Mai Linh Express",
                        License_Plate = "29B-67890",
                        Description = "Xe khách 45 chỗ có điều hòa, tivi"
                    },
                    new Transportation {
                        Type = TransportationType.Plane,
                        Name = "Vietnam Airlines",
                        Capacity = 150,
                        Provider = "Vietnam Airlines",
                        License_Plate = "VN-A123",
                        Description = "Máy bay thương mại"
                    },
                    new Transportation {
                        Type = TransportationType.Train,
                        Name = "Tàu SE1",
                        Capacity = 30,
                        Provider = "Đường sắt Việt Nam",
                        License_Plate = "SE1",
                        Description = "Tàu hỏa giường nằm khoang 4"
                    },
                    new Transportation {
                        Type = TransportationType.Boat,
                        Name = "Du thuyền Phoenix",
                        Capacity = 20,
                        Provider = "Phoenix Cruise",
                        License_Plate = "HL-001",
                        Description = "Du thuyền cao cấp trên vịnh Hạ Long"
                    }
                };

                context.Transportations.AddRange(transportations);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTourGuidesAsync(TourDbContext context)
        {
            if (!context.TourGuides.Any())
            {
                var tourGuides = new List<TourGuide>
                {
                    new TourGuide {
                        FullName = "Nguyễn Văn Minh",
                        Phone = "0987654321",
                        Email = "nguyenvanminh@email.com",
                        Experience = "5 năm kinh nghiệm hướng dẫn tour miền Bắc, thành thạo tiếng Anh"
                    },
                    new TourGuide {
                        FullName = "Trần Thị Lan",
                        Phone = "0976543210",
                        Email = "tranthilan@email.com",
                        Experience = "7 năm kinh nghiệm hướng dẫn tour miền Trung, thành thạo tiếng Anh và Pháp"
                    },
                    new TourGuide {
                        FullName = "Lê Hoàng Nam",
                        Phone = "0965432109",
                        Email = "lehoangnam@email.com",
                        Experience = "3 năm kinh nghiệm hướng dẫn tour miền Nam, thành thạo tiếng Anh"
                    },
                    new TourGuide {
                        FullName = "Phạm Thị Mai",
                        Phone = "0954321098",
                        Email = "phamthimai@email.com",
                        Experience = "6 năm kinh nghiệm hướng dẫn tour biển đảo, thành thạo tiếng Anh và Hàn"
                    }
                };

                context.TourGuides.AddRange(tourGuides);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCustomersAsync(TourDbContext context)
        {
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer {
                        FullName = "Nguyễn Thị Hoa",
                        Email = "nguyenthihoa@email.com",
                        Password = "hashedpassword123", // Trong thực tế cần hash password
                        Phone = "0901234567"
                    },
                    new Customer {
                        FullName = "Trần Văn Hùng",
                        Email = "tranvanhung@email.com",
                        Password = "hashedpassword456",
                        Phone = "0912345678"
                    },
                    new Customer {
                        FullName = "Lê Thị Lan",
                        Email = "lethilan@email.com",
                        Password = "hashedpassword789",
                        Phone = "0923456789"
                    }
                };

                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedItinerariesAsync(TourDbContext context)
        {
            if (!context.Itineraries.Any())
            {
                var tours = await context.Tours.ToListAsync();

                if (tours.Any())
                {
                    var halongTour = tours.First(t => t.Name_Tour.Contains("Hạ Long"));

                    var itineraries = new List<Itinerary>
                    {
                        new Itinerary {
                            ID_Tour = halongTour.ID_Tour,
                            Day_Number = 1,
                            Title = "Ngày 1: Hà Nội - Hạ Long - Du ngoạn vịnh",
                            Description = "6:00 Khởi hành từ Hà Nội. 9:00 Đến Hạ Long, lên du thuyền. 12:00 Ăn trưa trên thuyền. 14:00 Tham quan hang Sửng Sốt. 16:00 Tắm biển tại đảo Titop. 19:00 Ăn tối và nghỉ đêm trên thuyền."
                        },
                        new Itinerary {
                            ID_Tour = halongTour.ID_Tour,
                            Day_Number = 2,
                            Title = "Ngày 2: Vịnh Hạ Long - Hà Nội",
                            Description = "7:00 Ăn sáng trên thuyền. 8:30 Tham quan làng chài Cửa Vạn. 10:00 Xuống thuyền, về Hà Nội. 13:00 Về đến Hà Nội, kết thúc tour."
                        }
                    };

                    context.Itineraries.AddRange(itineraries);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedTourImagesAsync(TourDbContext context)
        {
            if (!context.TourImages.Any())
            {
                var tours = await context.Tours.Take(3).ToListAsync();

                if (tours.Any())
                {
                    var tourImages = new List<TourImage>
                    {
                        new TourImage {
                            ID_Tour = tours[0].ID_Tour,
                            Image_URL = "/img/halong1.jpg",
                            Caption = "Vịnh Hạ Long tuyệt đẹp"
                        },
                        new TourImage {
                            ID_Tour = tours[0].ID_Tour,
                            Image_URL = "/img/halong2.jpg",
                            Caption = "Du thuyền trên vịnh Hạ Long"
                        },
                        new TourImage {
                            ID_Tour = tours[1].ID_Tour,
                            Image_URL = "/img/hoian1.jpg",
                            Caption = "Phố cổ Hội An về đêm"
                        },
                        new TourImage {
                            ID_Tour = tours[2].ID_Tour,
                            Image_URL = "/img/sapa1.jpg",
                            Caption = "Ruộng bậc thang Sapa"
                        }
                    };

                    context.TourImages.AddRange(tourImages);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedTourAccommodationsAsync(TourDbContext context)
        {
            if (!context.TourAccommodations.Any())
            {
                var tours = await context.Tours.Take(3).ToListAsync();
                var accommodations = await context.Accommodations.Take(3).ToListAsync();

                if (tours.Any() && accommodations.Any())
                {
                    var tourAccommodations = new List<TourAccommodation>
                    {
                        new TourAccommodation {
                            ID_Tour = tours[0].ID_Tour,
                            ID_Accommodation = accommodations[0].ID_Accommodation,
                            Day_Number = 1,
                            Nights = 1,
                            Checkin_Time = new TimeSpan(14, 0, 0),
                            Checkout_Time = new TimeSpan(12, 0, 0)
                        },
                        new TourAccommodation {
                            ID_Tour = tours[1].ID_Tour,
                            ID_Accommodation = accommodations[1].ID_Accommodation,
                            Day_Number = 1,
                            Nights = 2,
                            Checkin_Time = new TimeSpan(14, 0, 0),
                            Checkout_Time = new TimeSpan(12, 0, 0)
                        }
                    };

                    context.TourAccommodations.AddRange(tourAccommodations);
                    await context.SaveChangesAsync();
                }
            }
        }

        private static async Task SeedTourTransportsAsync(TourDbContext context)
        {
            if (!context.TourTransports.Any())
            {
                var tours = await context.Tours.ToListAsync();
                var transports = await context.Transportations.ToListAsync();

                if (tours.Any() && transports.Any())
                {
                    var tourTransports = new List<TourTransport>();

                    // Chỉ thêm nếu có ít nhất 1 tour và 1 transport
                    if (tours.Count >= 1 && transports.Count >= 1)
                    {
                        tourTransports.Add(new TourTransport
                        {
                            ID_Tour = tours[0].ID_Tour,
                            ID_Transport = transports[0].ID_Transport,
                            Day_Number = 1,
                            From_Location = "Hà Nội",
                            To_Location = "Hạ Long",
                            Departure_Time = new TimeSpan(6, 0, 0),
                            Arrival_Time = new TimeSpan(9, 0, 0)
                        });
                    }

                    // Chỉ thêm du thuyền nếu có ít nhất 5 phương tiện
                    if (tours.Count >= 1 && transports.Count >= 5)
                    {
                        tourTransports.Add(new TourTransport
                        {
                            ID_Tour = tours[0].ID_Tour,
                            ID_Transport = transports[4].ID_Transport, // Du thuyền
                            Day_Number = 1,
                            From_Location = "Cảng Hạ Long",
                            To_Location = "Vịnh Hạ Long",
                            Departure_Time = new TimeSpan(9, 30, 0),
                            Arrival_Time = new TimeSpan(17, 0, 0)
                        });
                    }

                    if (tourTransports.Any())
                    {
                        context.TourTransports.AddRange(tourTransports);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        private static async Task SeedTourGuideAssignmentsAsync(TourDbContext context)
        {
            if (!context.TourGuideAssignments.Any())
            {
                var tours = await context.Tours.Take(3).ToListAsync();
                var guides = await context.TourGuides.Take(3).ToListAsync();

                if (tours.Any() && guides.Any())
                {
                    var assignments = new List<TourGuideAssignment>
                    {
                        new TourGuideAssignment {
                            ID_Tour = tours[0].ID_Tour,
                            ID_Guide = guides[0].ID_Guide,
                            Start_Date = DateTime.Now.AddDays(7),
                            End_Date = DateTime.Now.AddDays(9)
                        },
                        new TourGuideAssignment {
                            ID_Tour = tours[1].ID_Tour,
                            ID_Guide = guides[1].ID_Guide,
                            Start_Date = DateTime.Now.AddDays(14),
                            End_Date = DateTime.Now.AddDays(17)
                        },
                        new TourGuideAssignment {
                            ID_Tour = tours[2].ID_Tour,
                            ID_Guide = guides[2].ID_Guide,
                            Start_Date = DateTime.Now.AddDays(21),
                            End_Date = DateTime.Now.AddDays(24)
                        }
                    };

                    context.TourGuideAssignments.AddRange(assignments);
                    await context.SaveChangesAsync();
                }
            }
        }


    }
}