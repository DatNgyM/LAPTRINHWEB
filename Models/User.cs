using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAPTRINHWEB.Models
{
    public class User
    {
        [Key]
        [Column("ID_User")]
        public int ID_User { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        [Column("Full_Name")]
        public string Full_Name { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(255)]
        [Column("Password_Hash")]
        public string Password_Hash { get; set; }

        [StringLength(15)]
        [Column("Phone")]
        public string Phone { get; set; }

        [StringLength(500)]
        [Column("Address")]
        public string Address { get; set; }

        [Column("Date_Of_Birth")]
        [DataType(DataType.Date)]
        public DateTime? Date_Of_Birth { get; set; }

        [StringLength(10)]
        [Column("Gender")]
        public string Gender { get; set; }

        [StringLength(500)]
        [Column("Avatar")]
        public string Avatar { get; set; }

        [Required]
        [Column("ID_Role")]
        public int ID_Role { get; set; }

        [Column("Created_Date")]
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [Column("Last_Login")]
        public DateTime? Last_Login { get; set; }

        [Column("Is_Active")]
        public bool Is_Active { get; set; } = true;

        [Column("Email_Verified")]
        public bool Email_Verified { get; set; } = false;

        // Navigation properties
        [ForeignKey("ID_Role")]
        public Role Role { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}