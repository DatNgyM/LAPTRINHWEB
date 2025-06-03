using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAPTRINHWEB.Models
{
    public class Role
    {
        [Key]
        [Column("ID_Role")]
        public int ID_Role { get; set; }

        [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
        [StringLength(50)]
        [Column("Role_Name")]
        public string Role_Name { get; set; }

        [StringLength(200)]
        [Column("Description")]
        public string Description { get; set; }

        [Column("Created_Date")]
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [Column("Is_Active")]
        public bool Is_Active { get; set; } = true;

        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}