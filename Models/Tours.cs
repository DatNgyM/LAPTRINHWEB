using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public partial class Tour
    {
        [Key]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("Name_Tour")]
        public string Name_Tour { get; set; }
        
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        [Required]
        [Column("Duration")]
        public int Duration { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("Start_Location")]
        public string Start_Location { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("End_Location")]
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
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<TourImage> TourImages { get; set; } = new List<TourImage>();
        
        // Computed property - Giá sau giảm
        [NotMapped]
        public decimal FinalPrice => Price - (Discount ?? 0);
        
        // Computed property - Ảnh chính (ảnh đầu tiên)
        [NotMapped]
        public string MainImageUrl => TourImages?.FirstOrDefault()?.Image_URL ?? "/img/default-tour.jpg";
    }
    
    public enum TourStatus
    {
        Active = 0,
        Inactive = 1
    }
}