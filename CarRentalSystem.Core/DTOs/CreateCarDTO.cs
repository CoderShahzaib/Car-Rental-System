using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.DTOs
{
    public class CreateCarDTO
    {
        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal DailyRate { get; set; }

        [Required]
        [StringLength(20)]
        public string RegistrationNo { get; set; }

        public string Image { get; set; }
    }
}
