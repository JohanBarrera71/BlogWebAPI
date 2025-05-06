using System.ComponentModel.DataAnnotations;

namespace DemoLinkedInApi.DTOs
{
    public class RegisterDto : AccountBaseDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
             ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public new string? Password { get; set; }
    }
}
