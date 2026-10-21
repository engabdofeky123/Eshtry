using System.ComponentModel.DataAnnotations;

namespace Eshtry_Me.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}