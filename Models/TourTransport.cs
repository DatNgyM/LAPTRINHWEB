using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class TourTransport
    {
        [Key]
        [Column("ID_TourTransport")]
        public int ID_TourTransport { get; set; }
        
        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [Column("ID_Transport")]
        public int ID_Transport { get; set; }
        
        [Required]
        [Column("Day_Number")]
        public int Day_Number { get; set; }
        
        [StringLength(200)]
        [Column("From_Location")]
        public string From_Location { get; set; }
        
        [StringLength(200)]
        [Column("To_Location")]
        public string To_Location { get; set; }
        
        [Column("Departure_Time")]
        [DataType(DataType.Time)]
        public TimeSpan? Departure_Time { get; set; }
        
        [Column("Arrival_Time")]
        [DataType(DataType.Time)]
        public TimeSpan? Arrival_Time { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }
        
        [ForeignKey("ID_Transport")]
        public Transportation Transportation { get; set; }
    }
}