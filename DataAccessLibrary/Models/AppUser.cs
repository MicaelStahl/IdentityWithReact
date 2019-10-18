using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public override string UserName { get => base.UserName; set => base.UserName = Email; }

        [Required]
        [Display(Name = "Firstname")]
        [StringLength(20, ErrorMessage = "The Firstname can only hold a max length of {1} and min length of {2} characters.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        [StringLength(30, ErrorMessage = "The lastname can only hold a max length of {1} and min length of {2} characters.", MinimumLength = 2)]
        public string LastName { get; set; }
    }
}