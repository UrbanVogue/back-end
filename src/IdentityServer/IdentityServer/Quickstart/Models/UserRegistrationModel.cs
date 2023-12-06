using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class UserRegistrationModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}
