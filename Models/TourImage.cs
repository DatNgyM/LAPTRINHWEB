using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Models
{
    public class TourImage
    {
        [Key]
        [Column("ID_Image")]
        public int ID_Image { get; set; }
        
        [Required]
        [Column("ID_Tour")]
        public int ID_Tour { get; set; }
        
        [Required]
        [Column("Image_URL")]
        [DataType(DataType.Text)]
        public string Image_URL { get; set; }
        
        [StringLength(255)]
        [Column("Caption")]
        public string Caption { get; set; }
        
        // Navigation property
        [ForeignKey("ID_Tour")]
        public Tour Tour { get; set; }
    }
}