using System;
using System.Collections.Generic;

namespace LAPTRINHWEB.Models
{
    public partial class Tour
    {
        public int ID_Tour { get; set; }
        public string Name_Tour { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfPeople { get; set; }
        public int NumberOfDays { get; set; }
        public int NumberOfNights { get; set; }
        public String Type_Tour { get; set; }
    }
}