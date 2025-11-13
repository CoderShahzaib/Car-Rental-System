using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }  // Added ID for EF Core primary key

        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CustomerCity { get; set; } = string.Empty;

        [Phone]
        public string MobileNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        // Navigation property — one customer can have many bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}
