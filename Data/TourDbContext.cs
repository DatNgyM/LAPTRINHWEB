using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LAPTRINHWEB.Models
{

    public class TourDbContext : IdentityDbContext<ApplicationUser>
    {
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options)
        {
        }

        // Business DbSets
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TourGuideAssignment> TourGuideAssignments { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<TourAccommodation> TourAccommodations { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TourTransport> TourTransports { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryLocation> ItineraryLocations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Full_Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.Created_Date).HasDefaultValueSql("GETDATE()");
            });

            // Seed Identity Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "TourGuide",
                    NormalizedName = "TOURGUIDE"
                }
            );

            // Configure Tour
            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.ID_Tour);
                entity.ToTable("Tours");
                entity.Property(e => e.Name_Tour).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasColumnType("ntext");
                entity.Property(e => e.Start_Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.End_Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");

                entity.HasIndex(e => e.Name_Tour);
            });

            // Configure TourGuide
            modelBuilder.Entity<TourGuide>(entity =>
            {
                entity.HasKey(e => e.ID_Guide);
                entity.ToTable("TourGuides");
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure TourGuideAssignment
            modelBuilder.Entity<TourGuideAssignment>(entity =>
            {
                entity.HasKey(e => e.ID_Assignment);
                entity.ToTable("TourGuideAssignments");

                entity.HasOne(e => e.Tour)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TourGuide)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Guide)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Accommodation
            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.HasKey(e => e.ID_Accommodation);
                entity.ToTable("Accommodations");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            // Configure TourAccommodation
            modelBuilder.Entity<TourAccommodation>(entity =>
            {
                entity.HasKey(e => e.ID_TourAcc);
                entity.ToTable("TourAccommodations");

                entity.HasOne(e => e.Tour)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Accommodation)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Accommodation)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Transportation
            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.ID_Transport);
                entity.ToTable("Transportations");
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Provider).HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            // Configure TourTransport
            modelBuilder.Entity<TourTransport>(entity =>
            {
                entity.HasKey(e => e.ID_TourTransport);
                entity.ToTable("TourTransports");

                entity.HasOne(e => e.Tour)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Transportation)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Transport)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Itinerary
            modelBuilder.Entity<Itinerary>(entity =>
            {
                entity.HasKey(e => e.ID_Itinerary);
                entity.ToTable("Itineraries");
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.HasOne(e => e.Tour)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ItineraryLocation
            modelBuilder.Entity<ItineraryLocation>(entity =>
            {
                entity.HasKey(e => e.ID_Item);
                entity.ToTable("ItineraryLocations");

                entity.HasOne(e => e.Itinerary)
                    .WithMany(i => i.ItineraryLocations)
                    .HasForeignKey(e => e.ID_Itinerary)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Location)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Location)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.ID_Location);
                entity.ToTable("Locations");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            // Configure TourImage
            modelBuilder.Entity<TourImage>(entity =>
            {
                entity.HasKey(e => e.ID_Image);
                entity.ToTable("TourImages");
                entity.Property(e => e.Image_URL).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Caption).HasMaxLength(200);

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.TourImages)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Booking - Cập nhật để dùng ApplicationUser
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.ID_Booking);
                entity.ToTable("Bookings");
                entity.Property(e => e.Total_Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Booking_Date).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Note).HasColumnType("ntext");

                // Sử dụng UserId (string) thay vì ID_Customer (int)
                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Tour)
                    .WithMany()
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ID_Payment);
                entity.ToTable("Payments");
                entity.Property(e => e.Payment_Date).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Paid_Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Method).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Booking)
                    .WithMany(b => b.Payments)
                    .HasForeignKey(e => e.ID_Booking)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure indexes for better performance
            modelBuilder.Entity<Tour>()
                .HasIndex(t => t.Name_Tour);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.Booking_Date);

            modelBuilder.Entity<TourGuide>()
                .HasIndex(tg => tg.Email);

            modelBuilder.Entity<Location>()
                .HasIndex(l => l.Name);

            // Đổi tên bảng Identity để tránh xung đột
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
        }
    }
}