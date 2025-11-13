using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.DTOs
{
    public class CreateBookingDTO
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue)]

        public decimal TotalAmount { get; set; }
    }
}
