using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LAPTRINHWEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Full_Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;


        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}