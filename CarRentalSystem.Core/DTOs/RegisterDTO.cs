using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Core.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person name is required")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password can't be blank")]
        public string ConfirmPassword { get; set; }
    }
}
