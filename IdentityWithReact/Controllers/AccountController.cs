using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityWithReact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        #region D.I

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Unsure if needed
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenGeneration _token;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager, ILogger<AccountController> logger,
            ITokenGeneration token)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _token = token;
        }

        #endregion D.I

        #region UpdateToken

        /// <summary>
        /// Only ever used when the token used is outdated.
        /// </summary>
        /// <param name="id">The ID of the active user.</param>
        /// <returns></returns>
        [AllowAnonymous] // Forced to use this since the application otherwise tries to check the jwt-token.
        [HttpGet("update-token/{id}")]
        public async Task<IActionResult> UpdateToken([FromHeader]string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("id {id} was blank.");

                return BadRequest("The given ID was blank.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"No user was found with given ID: {id}");

                return NotFound("No user was found with given ID.");
            }

            return Ok(new FrontEndUpdate { ActiveId = user.Id, Email = user.Email, JwtToken = await _token.JwtTokenGeneration() });
        }

        #endregion UpdateToken

        #region SignIn / SignOut

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]SignIn signIn)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid credentials: One or more fields were invalid.");

                ModelState.AddModelError(string.Empty, "Invalid credentials: One or more fields were invalid.");

                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(signIn.Email);

            if (user == null)
            {
                _logger.LogWarning($"Unexpected error: No user with given email {signIn.Email} exists.", signIn.Email);

                ModelState.AddModelError(string.Empty, $"Unexpected error: No user with given email {signIn.Email} eixsts.");

                return NotFound($"Unexpected error: No user with given email {signIn.Email} exists.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest("Username or password were invalid.");
            }

            return Ok(new FrontEndUpdate
            {
                Email = user.Email,
                ActiveId = user.Id,
                Message = "User was successfully signed in",
                JwtToken = await _token.JwtTokenGeneration(),
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        [HttpGet("sign-out/{id}")]
        public async Task<IActionResult> SignOut([FromRoute]string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("ID {id} is blank.");

                return BadRequest("Could not sign out user: The active users ID is blank.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                _logger.LogError($"User could not be found with the given ID: {id}");

                return NotFound("No user was found with the active users ID.");
            }

            await _signInManager.SignOutAsync();

            if (_signInManager.IsSignedIn(User))
            {
                _logger.LogError($"User {User} could not be logged out.");

                return BadRequest("Something went wrong when logging out user. Please try again.");
            }
            else
            {
                _logger.LogInformation("User successfully logged out.");

                return Ok("User was successfully logged out.");
            }
        }

        #endregion SignIn / SignOut

        #region Create

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(message: "User {user} was invalid.", user);

                return BadRequest(ModelState);
            }

            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                _logger.LogWarning(message: new IdentityErrorDescriber().DuplicateEmail(user.Email).Description, "Unexpected error: The requested email is already in use.", user.Email);

                return BadRequest(new IdentityErrorDescriber().DuplicateEmail(user.Email).Description);
            }

            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, user.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            var roleResult = user.IsAdmin ?
                await _userManager.AddToRolesAsync(user, new string[] { "Administrator", "NormalUser" }) :
                await _userManager.AddToRoleAsync(user, "NormalUser");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok("User was successfully created.");
        }

        // Send email method here later.

        #endregion Create

        #region Find

        [HttpPost("find-one")]
        public async Task<IActionResult> Get([FromBody]GetUserVM getUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(getUser.UserId))
                {
                    _logger.LogError("UserId {getUser.UserId} was blank.", getUser.UserId);

                    ModelState.AddModelError(string.Empty, "Unexpected error: The given ID was blank.");

                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(getUser.ActiveId))
                {
                    _logger.LogError("ActiveId {getUser.ActiveId} was blank.", getUser.ActiveId);

                    ModelState.AddModelError(string.Empty, "Unexpected error: The active ID was blank.");

                    return BadRequest(ModelState);
                }

                var activeUser = await _userManager.FindByIdAsync(getUser.ActiveId);

                if (activeUser == null)
                {
                    _logger.LogError($"No user was found with ID {getUser.ActiveId}");

                    ModelState.AddModelError(string.Empty, $"Unexpected error: No user was found with ID: {getUser.ActiveId}");

                    return BadRequest(ModelState);
                }

                //var result = await _userManager.VerifyUserTokenAsync(activeUser, "Default", "authentication-backend", getUser.UserToken);

                //if (!result)
                //{
                //    _logger.LogError("Token {UserToken} was invalid.");

                //    ModelState.AddModelError(string.Empty, new IdentityErrorDescriber().InvalidToken().Description);

                //    return BadRequest("The token is outdated.");
                //}

                var user = await _userManager.FindByIdAsync(getUser.UserId);

                if (user == null)
                {
                    _logger.LogError($"User was not found: No user found with given id: {getUser.UserId}");

                    ModelState.AddModelError(string.Empty, $"Unexpected error occurred: No user with ID {getUser.UserId} was found.");

                    return NotFound("Unexpected error occurred: No user was found.");
                }

                var frontUser = new FrontUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsAdmin = user.IsAdmin,
                    Verification = new FrontVerification
                    {
                        ActiveId = activeUser.Id,
                        //UserToken = await _token.UserTokenGeneration(activeUser),
                        JwtToken = await _token.JwtTokenGeneration(),
                        Roles = await _userManager.GetRolesAsync(activeUser)
                    }
                };
                return Ok(frontUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-all")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = new Users { JwtToken = await _token.JwtTokenGeneration() };

                var userList = await _userManager.Users.ToListAsync();

                foreach (var user in userList)
                {
                    users.UserList.Add(new FrontUser
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        IsAdmin = user.IsAdmin,
                    });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex.InnerException, message: ex.Message, ex);

                return BadRequest(ex.Message);
            }
        }

        #endregion Find

        #region Edit

        // ToDo

        #endregion Edit
    }
}