using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityWithReact.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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

        #region Create

        [AllowAnonymous]
        [HttpGet("register")]
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

            return Ok(new JwtTokenWithMessage { Message = "User was successfully created.", JwtToken = await _token.JwtTokenGeneration() });
        }

        // Send email method here later.

        #endregion Create

        #region Find

        [HttpPost("find-one")]
        public async Task<IActionResult> Get([FromBody]GetUserVM getUser)
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

            return Ok(
                new FrontUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
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
                });
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
                        UserName = user.UserName,
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