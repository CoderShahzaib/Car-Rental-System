using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Core.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public Car? Car { get; set; }
        public Customer? Customer { get; set; }
    }
}
