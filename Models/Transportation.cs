using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace LAPTRINHWEB.Models
{
    public class Transportation
    {
        [Key]
        [Column("ID_Transport")]
        public int ID_Transport { get; set; }
        
        [Column("Type")]
        public TransportationType Type { get; set; }
        
        [Required]
        [StringLength(200)]
        [Column("Name")]
        public string Name { get; set; }
        
        [Column("Capacity")]
        public int? Capacity { get; set; }
        
        [StringLength(200)]
        [Column("Provider")]
        public string Provider { get; set; }
        
        [StringLength(20)]
        [Column("License_Plate")]
        public string License_Plate { get; set; }
        
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        // Navigation properties
        public ICollection<TourTransport> TourTransports { get; set; } = new List<TourTransport>();
    }
    
    public enum TransportationType
    {
        Bus = 0,
        Plane = 1,
        Boat = 2,
        Train = 3,
        Car = 4,
        Motorbike = 5
    }
}