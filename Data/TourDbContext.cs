using Microsoft.EntityFrameworkCore;
using LAPTRINHWEB.Models;

namespace LAPTRINHWEB.Data
{
    public class TourDbContext : DbContext
    {
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ItineraryLocation> ItineraryLocations { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<TourAccommodation> TourAccommodations { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TourTransport> TourTransports { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<TourGuideAssignment> TourGuideAssignments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.ID_Customer);
                entity.ToTable("Customer");

                entity.Property(e => e.ID_Customer)
                    .HasColumnName("ID_Customer")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FullName");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Email");

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Password");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("Phone");
            });

            // Cấu hình Tour - QUAN TRỌNG: Thêm navigation properties
            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.ID_Tour);
                entity.ToTable("Tour");

                entity.Property(e => e.ID_Tour)
                    .HasColumnName("ID_Tour")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name_Tour)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Name_Tour");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasColumnType("text");

                entity.Property(e => e.Duration)
                    .IsRequired()
                    .HasColumnName("Duration");

                entity.Property(e => e.Start_Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Start_Location");

                entity.Property(e => e.End_Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("End_Location");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Discount)
                    .HasColumnName("Discount")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Max_Capacity)
                    .IsRequired()
                    .HasColumnName("Max_Capacity");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasConversion<int>()
                    .HasDefaultValue(TourStatus.Active);

                // THÊM: Cấu hình navigation properties cho Tour
                entity.HasMany(t => t.TourImages)
                    .WithOne(ti => ti.Tour)
                    .HasForeignKey(ti => ti.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Itineraries)
                    .WithOne(i => i.Tour)
                    .HasForeignKey(i => i.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Bookings)
                    .WithOne(b => b.Tour)
                    .HasForeignKey(b => b.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TourAccommodations)
                    .WithOne(ta => ta.Tour)
                    .HasForeignKey(ta => ta.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TourTransports)
                    .WithOne(tt => tt.Tour)
                    .HasForeignKey(tt => tt.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TourGuideAssignments)
                    .WithOne(tga => tga.Tour)
                    .HasForeignKey(tga => tga.ID_Tour)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình TourImage
            modelBuilder.Entity<TourImage>(entity =>
            {
                entity.HasKey(e => e.ID_Image);
                entity.ToTable("TourImage");

                entity.Property(e => e.ID_Image)
                    .HasColumnName("ID_Image")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.Image_URL)
                    .IsRequired()
                    .HasColumnName("Image_URL")
                    .HasColumnType("text");

                entity.Property(e => e.Caption)
                    .HasMaxLength(255)
                    .HasColumnName("Caption");
            });

            // Cấu hình Itinerary
            modelBuilder.Entity<Itinerary>(entity =>
            {
                entity.HasKey(e => e.ID_Itinerary);
                entity.ToTable("Itinerary");

                entity.Property(e => e.ID_Itinerary)
                    .HasColumnName("ID_Itinerary")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.Day_Number)
                    .IsRequired()
                    .HasColumnName("Day_Number");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Title");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasColumnType("text");
            });

            // Cấu hình Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.ID_Location);
                entity.ToTable("Location");

                entity.Property(e => e.ID_Location)
                    .HasColumnName("ID_Location")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Name");

                entity.Property(e => e.Address)
                    .HasColumnName("Address")
                    .HasColumnType("text");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasColumnType("text");
            });

            // Cấu hình ItineraryLocation (Many-to-Many)
            modelBuilder.Entity<ItineraryLocation>(entity =>
            {
                entity.HasKey(e => e.ID_Item);
                entity.ToTable("ItineraryLocation");

                entity.Property(e => e.ID_Item)
                    .HasColumnName("ID_Item")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Itinerary)
                    .IsRequired()
                    .HasColumnName("ID_Itinerary");

                entity.Property(e => e.ID_Location)
                    .IsRequired()
                    .HasColumnName("ID_Location");

                entity.Property(e => e.Visit_Time)
                    .HasColumnName("Visit_Time")
                    .HasColumnType("time");

                // Quan hệ: ItineraryLocation belongs to Itinerary
                entity.HasOne(d => d.Itinerary)
                    .WithMany(p => p.ItineraryLocations)
                    .HasForeignKey(d => d.ID_Itinerary)
                    .OnDelete(DeleteBehavior.Cascade);

                // Quan hệ: ItineraryLocation belongs to Location
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ItineraryLocations)
                    .HasForeignKey(d => d.ID_Location)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình Accommodation
            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.HasKey(e => e.ID_Accommodation);
                entity.ToTable("Accommodation");

                entity.Property(e => e.ID_Accommodation)
                    .HasColumnName("ID_Accommodation")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Name");

                entity.Property(e => e.Type)
                    .HasColumnName("Type")
                    .HasConversion<int>();

                entity.Property(e => e.Address)
                    .HasColumnName("Address")
                    .HasColumnType("text");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("Phone");

                entity.Property(e => e.Star_Rating)
                    .HasColumnName("Star_Rating");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasColumnType("text");
            });

            // Cấu hình TourAccommodation (Many-to-Many)
            modelBuilder.Entity<TourAccommodation>(entity =>
            {
                entity.HasKey(e => e.ID_TourAcc);
                entity.ToTable("TourAccommodation");

                entity.Property(e => e.ID_TourAcc)
                    .HasColumnName("ID_TourAcc")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.ID_Accommodation)
                    .IsRequired()
                    .HasColumnName("ID_Accommodation");

                entity.Property(e => e.Day_Number)
                    .IsRequired()
                    .HasColumnName("Day_Number");

                entity.Property(e => e.Nights)
                    .IsRequired()
                    .HasColumnName("Nights");

                entity.Property(e => e.Checkin_Time)
                    .HasColumnName("Checkin_Time")
                    .HasColumnType("time");

                entity.Property(e => e.Checkout_Time)
                    .HasColumnName("Checkout_Time")
                    .HasColumnType("time");

                // Quan hệ: TourAccommodation belongs to Accommodation
                entity.HasOne(d => d.Accommodation)
                    .WithMany(p => p.TourAccommodations)
                    .HasForeignKey(d => d.ID_Accommodation)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình Transportation
            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.ID_Transport);
                entity.ToTable("Transportation");

                entity.Property(e => e.ID_Transport)
                    .HasColumnName("ID_Transport")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .HasColumnName("Type")
                    .HasConversion<int>();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Name");

                entity.Property(e => e.Capacity)
                    .HasColumnName("Capacity");

                entity.Property(e => e.Provider)
                    .HasMaxLength(200)
                    .HasColumnName("Provider");

                entity.Property(e => e.License_Plate)
                    .HasMaxLength(20)
                    .HasColumnName("License_Plate");

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasColumnType("text");
            });

            // Cấu hình TourTransport (Many-to-Many)
            modelBuilder.Entity<TourTransport>(entity =>
            {
                entity.HasKey(e => e.ID_TourTransport);
                entity.ToTable("TourTransport");

                entity.Property(e => e.ID_TourTransport)
                    .HasColumnName("ID_TourTransport")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.ID_Transport)
                    .IsRequired()
                    .HasColumnName("ID_Transport");

                entity.Property(e => e.Day_Number)
                    .IsRequired()
                    .HasColumnName("Day_Number");

                entity.Property(e => e.From_Location)
                    .HasMaxLength(200)
                    .HasColumnName("From_Location");

                entity.Property(e => e.To_Location)
                    .HasMaxLength(200)
                    .HasColumnName("To_Location");

                entity.Property(e => e.Departure_Time)
                    .HasColumnName("Departure_Time")
                    .HasColumnType("time");

                entity.Property(e => e.Arrival_Time)
                    .HasColumnName("Arrival_Time")
                    .HasColumnType("time");

                // Quan hệ: TourTransport belongs to Transportation
                entity.HasOne(d => d.Transportation)
                    .WithMany(p => p.TourTransports)
                    .HasForeignKey(d => d.ID_Transport)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình TourGuide
            modelBuilder.Entity<TourGuide>(entity =>
            {
                entity.HasKey(e => e.ID_Guide);
                entity.ToTable("TourGuide");

                entity.Property(e => e.ID_Guide)
                    .HasColumnName("ID_Guide")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FullName");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("Phone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Email");

                entity.Property(e => e.Experience)
                    .HasColumnName("Experience")
                    .HasColumnType("text");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Cấu hình TourGuideAssignment (Many-to-Many)
            modelBuilder.Entity<TourGuideAssignment>(entity =>
            {
                entity.HasKey(e => e.ID_Assignment);
                entity.ToTable("TourGuideAssignment");

                entity.Property(e => e.ID_Assignment)
                    .HasColumnName("ID_Assignment")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.ID_Guide)
                    .IsRequired()
                    .HasColumnName("ID_Guide");

                entity.Property(e => e.Start_Date)
                    .IsRequired()
                    .HasColumnName("Start_Date")
                    .HasColumnType("date");

                entity.Property(e => e.End_Date)
                    .IsRequired()
                    .HasColumnName("End_Date")
                    .HasColumnType("date");

                // Quan hệ: TourGuideAssignment belongs to TourGuide
                entity.HasOne(d => d.TourGuide)
                    .WithMany(p => p.TourGuideAssignments)
                    .HasForeignKey(d => d.ID_Guide)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.ID_Booking);
                entity.ToTable("Booking");

                entity.Property(e => e.ID_Booking)
                    .HasColumnName("ID_Booking")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Customer)
                    .IsRequired()
                    .HasColumnName("ID_Customer");

                entity.Property(e => e.ID_Tour)
                    .IsRequired()
                    .HasColumnName("ID_Tour");

                entity.Property(e => e.Booking_Date)
                    .IsRequired()
                    .HasColumnName("Booking_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Number_Adults)
                    .IsRequired()
                    .HasColumnName("Number_Adults");

                entity.Property(e => e.Number_Children)
                    .HasColumnName("Number_Children")
                    .HasDefaultValue(0);

                entity.Property(e => e.Total_Price)
                    .IsRequired()
                    .HasColumnName("Total_Price")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasConversion<int>()
                    .HasDefaultValue(BookingStatus.Pending);

                entity.Property(e => e.Note)
                    .HasColumnName("Note")
                    .HasColumnType("text");

                // Quan hệ: Booking belongs to Customer
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ID_Customer)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ID_Payment);
                entity.ToTable("Payment");

                entity.Property(e => e.ID_Payment)
                    .HasColumnName("ID_Payment")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ID_Booking)
                    .IsRequired()
                    .HasColumnName("ID_Booking");

                entity.Property(e => e.Method)
                    .HasColumnName("Method")
                    .HasConversion<int>();

                entity.Property(e => e.Paid_Amount)
                    .IsRequired()
                    .HasColumnName("Paid_Amount")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Payment_Date)
                    .IsRequired()
                    .HasColumnName("Payment_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasConversion<int>()
                    .HasDefaultValue(PaymentStatus.Waiting);

                // Quan hệ: Payment belongs to Booking
                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ID_Booking)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}