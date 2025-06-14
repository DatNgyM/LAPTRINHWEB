using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAPTRINHWEB.Models
{
    public class ItineraryDetail
    {
        [Key]
        public int ID_Detail { get; set; }

        [Required]
        public int ID_Itinerary { get; set; }

        [Required]
        [StringLength(50)]
        public string Time { get; set; }

        [Required]
        public string Activities { get; set; }

        // Navigation property: mỗi detail thuộc về 1 Itinerary
        [ForeignKey("ID_Itinerary")]
        public virtual Itinerary Itinerary { get; set; }
    }
}