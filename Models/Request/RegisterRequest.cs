using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.Request
{
    public class RegisterRequest
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password too short")]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public int roleId { get; set; }
    }
}
