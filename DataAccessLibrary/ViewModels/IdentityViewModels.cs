using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.ViewModels
{
    #region Frontend Models

    /// <summary>
    /// Contains variables used for verification to frontend.
    /// </summary>
    public class FrontVerification : ActiveUser
    {
        /// <summary>
        /// Used to restrict certain content on frontend.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// The data sent to the frontend.
    /// </summary>
    public class FrontUser
    {
        public FrontVerification Verification { get; set; }

        [Key]
        public string Id { get; set; }

        [Required]
        public string UserName { get => UserName; set => UserName = Email; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Indicates whether the user is an administrator or not.
        /// </summary>
        public bool IsAdmin { get; set; }
    }

    /// <summary>
    /// Contains a message and jwtToken update. Used after registration.
    /// </summary>
    public class JwtTokenWithMessage : Tokens
    {
        public string Message { get; set; }
    }

    /// <summary>
    /// Contains a list of users and a JwtToken.
    /// Only Admin can use this.
    /// </summary>
    public class Users : Tokens
    {
        /// <summary>
        /// A list of all Users in the application.
        /// </summary>
        public List<FrontUser> UserList { get; set; }
    }

    /// <summary>
    /// Used for updating front-end with new verification variables.
    /// </summary>
    public class FrontEndUpdate : ActiveUser
    {
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// A list containing all the roles the active user is in.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Used to give the user a message indicating the action was a success.
        /// </summary>
        public string Message { get; set; }
    }

    #endregion Frontend Models

    #region Backend Models

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

    /// <summary>
    /// Contains two tokens used for authentication for signed in users.
    /// <para>UserToken: Token used to verify user on backend. DEPRECATED</para>
    /// <para>JwtToken: Used to verify call between frontend and backend.</para>
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// User-token used to verify the active user. Currently not used.
        /// </summary>
        //public string UserToken { get; set; }

        /// <summary>
        /// Jwt-token used to verify user between backend and frontend calls.
        /// </summary>
        public string JwtToken { get; set; }
    }

    /// <summary>
    /// Contains the active users ID
    /// </summary>
    public class ActiveUser : Tokens
    {
        /// <summary>
        /// The active users ID. Will always be the same as the "UserId" except for when an admin wants to inspect a user.
        /// </summary>
        public string ActiveId { get; set; }
    }

    /// <summary>
    /// Contains the requested users ID and polymorphed with <see cref="ActiveUser"/>
    /// </summary>
    public class GetUserVM : ActiveUser
    {
        public string UserId { get; set; }
    }

    /// <summary>
    /// Contains a username (Email) and password for signing in.
    /// </summary>
    public class SignIn
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    #endregion Backend Models
}