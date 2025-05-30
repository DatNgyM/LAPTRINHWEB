using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class TourAccommodation
    {
        [Key]
        [Column("ID_TourAcc")]
        public int ID_TourAcc { get; set; }
        
        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [Column("ID_Accommodation")]
        public int ID_Accommodation { get; set; }
        
        [Required]
        [Column("Day_Number")]
        public int Day_Number { get; set; }
        
        [Required]
        [Column("Nights")]
        public int Nights { get; set; }
        
        [Column("Checkin_Time")]
        [DataType(DataType.Time)]
        public TimeSpan? Checkin_Time { get; set; }
        
        [Column("Checkout_Time")]
        [DataType(DataType.Time)]
        public TimeSpan? Checkout_Time { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }
        
        [ForeignKey("ID_Accommodation")]
        public Accommodation Accommodation { get; set; }
    }
}