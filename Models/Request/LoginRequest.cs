using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password too short")]
        public string Password { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public string PhoneNumber { get; set; }
    }
}
