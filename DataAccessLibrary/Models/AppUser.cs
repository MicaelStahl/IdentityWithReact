using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models
{
    /// <summary>
    /// Default ApplicationUser for this application.
    /// </summary>
    public class AppUser : IdentityUser
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public override string UserName { get; set; } /*{ get => base.UserName; set => base.UserName = Email; }*/

        [Required]
        [Display(Name = "Firstname")]
        [StringLength(20, ErrorMessage = "The Firstname can only hold a max length of {1} and min length of {2} characters.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        [StringLength(30, ErrorMessage = "The lastname can only hold a max length of {1} and min length of {2} characters.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Range(15, 110, ErrorMessage = "The allowed age is 15 to 110 years old.")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// A boolean value indicating whether the user is an admin or not.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}