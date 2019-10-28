using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.ViewModels
{
    /// <summary>
    /// User for registration purposes.
    /// </summary>
    public class RegisterUser : AppUser
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The passwords has to match.")]
        public string ConfirmPassword { get; set; }
    }
}