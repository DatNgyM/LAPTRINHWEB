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
        [Column("Name", TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column("Capacity")]
        public int? Capacity { get; set; }

        [StringLength(200)]
        [Column("Provider", TypeName = "nvarchar(200)")]
        public string Provider { get; set; }

        [StringLength(20)]
        [Column("License_Plate", TypeName = "varchar(20)")]
        public string License_Plate { get; set; }

        [Column("Description", TypeName = "ntext")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        // Navigation properties
        public ICollection<TourTransport> TourTransports { get; set; } = new List<TourTransport>();
    }

    public enum TransportationType
    {
        Bus = 0,
        Car = 1,
        Plane = 2,
        Train = 3,
        Boat = 4
    }
}