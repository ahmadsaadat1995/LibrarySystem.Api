using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Password Is Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, including at least 1 lowercase letter, 1 uppercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; }

        public string? Role { get; set; }

    }
}
