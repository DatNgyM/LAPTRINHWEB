using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class Booking
    {
        [Key]
        [Column("ID_Booking")]
        public int ID_Booking { get; set; }

        [Required]
        [Column("ID_Customer")]
        public int ID_Customer { get; set; }

        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }

        [Required]
        [Column("Booking_Date")]
        public DateTime Booking_Date { get; set; }

        [Required]
        [Column("Number_Adults")]
        public int Number_Adults { get; set; }

        [Column("Number_Children")]
        public int Number_Children { get; set; } = 0;

        [Required]
        [Column("Total_Price")]
        [DataType(DataType.Currency)]
        public decimal Total_Price { get; set; }

        [Column("Status")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Column("Note", TypeName = "ntext")]
        [DataType(DataType.Text)]
        public string Note { get; set; }

        // Navigation properties
        [ForeignKey("ID_Customer")]
        public Customer Customer { get; set; }

        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2,
        Completed = 3
    }
}