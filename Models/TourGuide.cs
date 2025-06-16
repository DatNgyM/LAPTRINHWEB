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
        [Column("FullName", TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        [Column("Phone", TypeName = "varchar(15)")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Column("Email", TypeName = "varchar(150)")]
        public string Email { get; set; }

        [Required]
        [Column("Experience", TypeName = "int")]
        public int Experience { get; set; }

        [StringLength(100)]
        [Column("Area", TypeName = "nvarchar(100)")]
        public string Area { get; set; }

        [StringLength(150)]
        [Column("Specialization", TypeName = "nvarchar(150)")]
        public string Specialization { get; set; }

        // Navigation properties
        public ICollection<TourGuideAssignment> TourGuideAssignments { get; set; } = new List<TourGuideAssignment>();
    }
}