using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class ItineraryLocation
    {
        [Key]
        [Column("ID_Item")]
        public int ID_Item { get; set; }
        
        [Required]
        [Column("ID_Itinerary")]
        public int ID_Itinerary { get; set; }
        
        [Required]
        [Column("ID_Location")]
        public int ID_Location { get; set; }
        
        [Column("Visit_Time")]
        [DataType(DataType.Time)]
        public TimeSpan? Visit_Time { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Itinerary")]
        public Itinerary Itinerary { get; set; }
        
        [ForeignKey("ID_Location")]
        public Location Location { get; set; }
    }
}