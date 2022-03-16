using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.DatabaseModels
{
    public class Users
    {
        public Users()
        {
            Orders = new HashSet<Order>();
            Drafts = new HashSet<Drafts>();
            Notifications = new HashSet<Notifications>();
        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [MinLength(5, ErrorMessage = "Password too short")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        //public WalletDetails Wallet { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Drafts> Drafts { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
    }
}
