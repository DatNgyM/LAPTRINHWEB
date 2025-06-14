using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace LAPTRINHWEB.Models
{
    public class Itinerary
    {
        [Key]
        [Column("ID_Itinerary")]
        public int ID_Itinerary { get; set; }

        [Required]
        public int ID_Tour { get; set; }

        [Required]
        [Column("Day_Number")]
        public int Day_Number { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Title", TypeName = "nvarchar(200)")]
        public string Title { get; set; }

        [ForeignKey("ID_Tour")]
        public virtual Tour Tour { get; set; }

        public ICollection<ItineraryDetail> Details { get; set; }
        public ICollection<ItineraryLocation> ItineraryLocations { get; set; } = new List<ItineraryLocation>();

        public Itinerary()
        {
            Details = new List<ItineraryDetail>();
            ItineraryLocations = new List<ItineraryLocation>();
        }
    }
}