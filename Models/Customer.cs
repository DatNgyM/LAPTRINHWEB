using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Customer
    {
        [Key]
        [Column("ID_Customer")]
        public int ID_Customer { get; set; }

        [Required]
        [StringLength(100)]
        [Column("FullName", TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Column("Email", TypeName = "varchar(150)")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Password", TypeName = "varchar(255)")]
        public string Password { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        [Column("Phone", TypeName = "varchar(15)")]
        public string Phone { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}