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

            // ===== CONFIGURE APPLICATION USER =====
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers");
                entity.Property(e => e.Full_Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.Created_Date).HasDefaultValueSql("GETDATE()");
            });

            // ===== CONFIGURE TOUR =====
            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.ID_Tour);
                entity.ToTable("Tours");
                entity.Property(e => e.Name_Tour).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasColumnType("ntext");
                entity.Property(e => e.Start_Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.End_Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.Max_Capacity).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(TourStatus.Active);

                entity.HasIndex(e => e.Name_Tour);
            });

            // ===== CONFIGURE TOUR GUIDE =====
            modelBuilder.Entity<TourGuide>(entity =>
            {
                entity.HasKey(e => e.ID_Guide);
                entity.ToTable("TourGuides");
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Experience).HasColumnType("ntext");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // ===== CONFIGURE TOUR GUIDE ASSIGNMENT =====
            modelBuilder.Entity<TourGuideAssignment>(entity =>
            {
                entity.HasKey(e => e.ID_Assignment);
                entity.ToTable("TourGuideAssignments");
                entity.Property(e => e.Start_Date).IsRequired();
                entity.Property(e => e.End_Date).IsRequired();

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.TourGuideAssignments)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TourGuide)
                    .WithMany(tg => tg.TourGuideAssignments)
                    .HasForeignKey(e => e.ID_Guide)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE ACCOMMODATION =====
            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.HasKey(e => e.ID_Accommodation);
                entity.ToTable("Accommodations");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Star_Rating).HasDefaultValue(null);
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            // ===== CONFIGURE TOUR ACCOMMODATION =====
            modelBuilder.Entity<TourAccommodation>(entity =>
            {
                entity.HasKey(e => e.ID_TourAcc);
                entity.ToTable("TourAccommodations");
                entity.Property(e => e.Day_Number).IsRequired();
                entity.Property(e => e.Nights).IsRequired();
                entity.Property(e => e.Checkin_Time).HasColumnType("time");
                entity.Property(e => e.Checkout_Time).HasColumnType("time");

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.TourAccommodations)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Accommodation)
                    .WithMany(a => a.TourAccommodations)
                    .HasForeignKey(e => e.ID_Accommodation)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE TRANSPORTATION =====
            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.ID_Transport);
                entity.ToTable("Transportations");
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Capacity).HasDefaultValue(null);
                entity.Property(e => e.Provider).HasMaxLength(200);
                entity.Property(e => e.License_Plate).HasMaxLength(20);
                entity.Property(e => e.Description).HasColumnType("ntext");
            });

            // ===== CONFIGURE TOUR TRANSPORT =====
            modelBuilder.Entity<TourTransport>(entity =>
            {
                entity.HasKey(e => e.ID_TourTransport);
                entity.ToTable("TourTransports");
                entity.Property(e => e.Day_Number).IsRequired();
                entity.Property(e => e.From_Location).HasMaxLength(200);
                entity.Property(e => e.To_Location).HasMaxLength(200);
                entity.Property(e => e.Departure_Time).HasColumnType("time");
                entity.Property(e => e.Arrival_Time).HasColumnType("time");

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.TourTransports)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Transportation)
                    .WithMany(tr => tr.TourTransports)
                    .HasForeignKey(e => e.ID_Transport)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE ITINERARY =====
            modelBuilder.Entity<Itinerary>(entity =>
            {
                entity.HasKey(e => e.ID_Itinerary);
                entity.ToTable("Itineraries");
                entity.Property(e => e.Day_Number).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.Itineraries)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE LOCATION =====
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.ID_Location);
                entity.ToTable("Locations");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.HasIndex(e => e.Name);
            });

            // ===== CONFIGURE ITINERARY LOCATION =====
            modelBuilder.Entity<ItineraryLocation>(entity =>
            {
                entity.HasKey(e => e.ID_Item);
                entity.ToTable("ItineraryLocations");

                entity.HasOne(e => e.Itinerary)
                    .WithMany(i => i.ItineraryLocations)
                    .HasForeignKey(e => e.ID_Itinerary)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Location)
                    .WithMany(l => l.ItineraryLocations)
                    .HasForeignKey(e => e.ID_Location)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE TOUR IMAGE =====
            modelBuilder.Entity<TourImage>(entity =>
            {
                entity.HasKey(e => e.ID_Image);
                entity.ToTable("TourImages");
                entity.Property(e => e.Image_URL).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Caption).HasMaxLength(255);

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.TourImages)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE BOOKING =====
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.ID_Booking);
                entity.ToTable("Bookings");
                entity.Property(e => e.Booking_Date).IsRequired();
                entity.Property(e => e.Number_Adults).IsRequired();
                entity.Property(e => e.Number_Children).HasDefaultValue(0);
                entity.Property(e => e.Total_Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasDefaultValue(BookingStatus.Pending);
                entity.Property(e => e.Note).HasColumnType("ntext");
                entity.Property(e => e.UserId).IsRequired(false); // Có thể null cho guest booking

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Tour)
                    .WithMany(t => t.Bookings)
                    .HasForeignKey(e => e.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Booking_Date);
            });

            // ===== CONFIGURE PAYMENT =====
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ID_Payment);
                entity.ToTable("Payments");
                entity.Property(e => e.Method).IsRequired();
                entity.Property(e => e.Paid_Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Payment_Date).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(PaymentStatus.Waiting);

                entity.HasOne(e => e.Booking)
                    .WithMany(b => b.Payments)
                    .HasForeignKey(e => e.ID_Booking)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== CONFIGURE IDENTITY TABLES =====
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

            // ===== ADDITIONAL INDEXES FOR PERFORMANCE =====
            modelBuilder.Entity<Tour>()
                .HasIndex(t => t.Status);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.Status);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.Payment_Date);

            modelBuilder.Entity<TourGuideAssignment>()
                .HasIndex(tga => new { tga.Start_Date, tga.End_Date });
        }
    }
}