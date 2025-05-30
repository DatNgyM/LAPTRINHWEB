using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class TourGuideAssignment
    {
        [Key]
        [Column("ID_Assignment")]
        public int ID_Assignment { get; set; }
        
        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [Column("ID_Guide")]
        public int ID_Guide { get; set; }
        
        [Required]
        [Column("Start_Date")]
        [DataType(DataType.Date)]
        public DateTime Start_Date { get; set; }
        
        [Required]
        [Column("End_Date")]
        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }
        
        [ForeignKey("ID_Guide")]
        public TourGuide TourGuide { get; set; }
    }
}