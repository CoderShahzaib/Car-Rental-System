using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal DailyRate { get; set; }
        public string RegistrationNo { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public ICollection<Booking>? Bookings { get; set; }
    }

}
