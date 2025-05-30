using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Location
    {
        [Key]
        [Column("ID_Location")]
        public int ID_Location { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }
        
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        // Navigation properties
        public ICollection<ItineraryLocation> ItineraryLocations { get; set; } = new List<ItineraryLocation>();
    }
}