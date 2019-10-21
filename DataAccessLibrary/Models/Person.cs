using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{
    /// <summary>
    /// Regular person used for this application.
    /// </summary>
    public class Person
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "The length of the firstname has to be between 2 to 20 letters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The length of the lastname has to be between 2 to 30 letters.")]
        public string LastName { get; set; }

        [Required]
        [Range(5, 110, ErrorMessage = "The age of the person has to be between 5 to 110 years old.")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone]
        [Required]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "Only numbers allowed.")] // Unsure if needed.
        [StringLength(12, MinimumLength = 4, ErrorMessage = "The length of the phonenumber has to be between 4 to 12 numbers.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "The character-length of the city cannot exceed 60 characters, nor be less than 2.")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "The postal code cannot exceed 12 characters, nor be less than 4.")]
        public string PostalCode { get; set; }
    }
}