using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.DTOs
{
    public class CreateCustomerDTO
    {

        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CustomerCity { get; set; } = string.Empty;

        [Phone]
        public string MobileNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
