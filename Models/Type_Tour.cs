using System;
using System.Collections.Generic;

namespace LAPTRINHWEB.Models
{
    public partial class Type_Tour
    {
        public int ID_TypeTour { get; set; }
        public string Name_TypeTour { get; set; } = null!;
        public string? Description { get; set; }
        
    }
}