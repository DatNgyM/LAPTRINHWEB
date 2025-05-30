using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Itinerary
    {
        [Key]
        [Column("ID_Itinerary")]
        public int ID_Itinerary { get; set; }
        
        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [Column("Day_Number")]
        public int Day_Number { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("Title")]
        public string Title { get; set; }
        
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }
        public ICollection<ItineraryLocation> ItineraryLocations { get; set; } = new List<ItineraryLocation>();
    }
}