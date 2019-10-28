using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
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

            return Ok("User was successfully created.");
        }

        #endregion Create
    }
}