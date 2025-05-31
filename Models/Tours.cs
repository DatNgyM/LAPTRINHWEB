using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LAPTRINHWEB.Models
{
    [Table("Tour")]
    public class Tour
    {
        [Key]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Name_Tour", TypeName = "nvarchar(200)")]
        public string Name_Tour { get; set; }

        [Column("Description", TypeName = "ntext")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [Column("Duration")]
        public int Duration { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Start_Location", TypeName = "nvarchar(100)")]
        public string Start_Location { get; set; }

        [Required]
        [StringLength(100)]
        [Column("End_Location", TypeName = "nvarchar(100)")]
        public string End_Location { get; set; }

        [Required]
        [Column("Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Column("Discount")]
        [DataType(DataType.Currency)]
        public decimal? Discount { get; set; }

        [Required]
        [Column("Max_Capacity")]
        public int Max_Capacity { get; set; }

        [Column("Status")]
        public TourStatus Status { get; set; } = TourStatus.Active;

        // Navigation properties
        public virtual ICollection<TourImage> TourImages { get; set; } = new List<TourImage>();
        public virtual ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<TourAccommodation> TourAccommodations { get; set; } = new List<TourAccommodation>();
        public virtual ICollection<TourTransport> TourTransports { get; set; } = new List<TourTransport>();
        public virtual ICollection<TourGuideAssignment> TourGuideAssignments { get; set; } = new List<TourGuideAssignment>();

        // Computed properties
        [NotMapped]
        public decimal FinalPrice => Price - (Discount ?? 0);

        [NotMapped]
        public string MainImageUrl => TourImages?.FirstOrDefault()?.Image_URL ?? "/img/default-tour.jpg";

        [NotMapped]
        public string DurationText => $"{Duration} ngày {Duration - 1} đêm";
    }

    public enum TourStatus
    {
        Active = 0,
        Inactive = 1
    }
}