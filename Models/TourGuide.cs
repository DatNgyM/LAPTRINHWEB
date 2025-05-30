using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class TourGuide
    {
        [Key]
        [Column("ID_Guide")]
        public int ID_Guide { get; set; }
        
        [Required]
        [StringLength(100)]
        [Column("FullName")]
        public string FullName { get; set; }
        
        [Required]
        [Phone]
        [StringLength(15)]
        [Column("Phone")]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("Experience")]
        [DataType(DataType.Text)]
        public string Experience { get; set; }
        
        // Navigation properties
        public ICollection<TourGuideAssignment> TourGuideAssignments { get; set; } = new List<TourGuideAssignment>();
    }
}