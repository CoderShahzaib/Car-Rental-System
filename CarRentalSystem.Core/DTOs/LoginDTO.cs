using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Person Name is required")]
        public string PersonName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
