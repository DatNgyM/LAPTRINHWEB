using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Accommodation
    {
        [Key]
        [Column("ID_Accommodation")]
        public int ID_Accommodation { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Type")]
        public AccommodationType Type { get; set; }
        
        [Column("Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }
        
        [StringLength(15)]
        [Column("Phone")]
        public string Phone { get; set; }
        
        [Range(1, 5)]
        [Column("Star_Rating")]
        public int? Star_Rating { get; set; }
        
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        // Navigation properties
        public ICollection<TourAccommodation> TourAccommodations { get; set; } = new List<TourAccommodation>();
    }
    
    public enum AccommodationType
    {
        Hotel = 0,
        Resort = 1,
        Homestay = 2,
        Hostel = 3,
        Villa = 4
    }
}