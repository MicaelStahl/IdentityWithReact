using BusinessLibrary.Interfaces;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Repositories
{
    public class TokenGeneration : ITokenGeneration
    {
        #region D.I

        private readonly UserManager<AppUser> _userManager;
        private readonly AppSettingsToken _token;

        public TokenGeneration(UserManager<AppUser> userManager,
            IOptions<AppSettingsToken> token)
        {
            _userManager = userManager;
            _token = token.Value;
        }

        #endregion D.I

        /// <summary>
        /// Creates a Jwt-token for calls from frontend to backend.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EncoderFallbackException"></exception>
        /// <exception cref="SecurityTokenEncryptionFailedException"></exception>
        /// <returns></returns>
        public async Task<string> JwtTokenGeneration()
        {
            try
            {
                // One way to write a Jwt-token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token.Secret));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:3000",
                    audience: "http://localhost:3000",
                    claims: new List<Claim>(),
                    // Default expiration-date for Jwt-token for this application
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signingCredentials
                    );

                return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(tokenOptions));

                #region Another Jwt-token

                //var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_token.Secret);
                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(),
                //    // Indicates how long the token is valid for
                //    Expires = DateTime.UtcNow.AddDays(1),
                //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                //};
                //var token = tokenHandler.CreateToken(tokenDescriptor);
                //return await Task.FromResult(tokenHandler.WriteToken(token));

                #endregion Another Jwt-token
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a token used to validate the active user before performing actions.
        /// </summary>
        /// <param name="user">The user to bind the token with.</param>
        /// <exception cref="IdentityErrorDescriber"></exception>
        /// <returns></returns>
        public async Task<string> UserTokenGeneration(AppUser user)
        {
            try
            {
                var token = await _userManager.GenerateUserTokenAsync(user, "Default", "authentication-backend");

                if (string.IsNullOrWhiteSpace(token))
                    throw new Exception(new IdentityErrorDescriber().InvalidToken().Description);

                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}