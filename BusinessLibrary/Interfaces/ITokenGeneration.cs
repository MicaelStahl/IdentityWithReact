using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Interfaces
{
    public interface ITokenGeneration
    {
        /// <summary>
        /// Crates a usertoken used to validate the active user before performing actions.
        /// </summary>
        /// <param name="user">The user to bind the token with.</param>
        /// <returns></returns>
        Task<string> UserTokenGeneration(AppUser user);

        /// <summary>
        /// Creates a Jwt-token for calls from frontend to backend.
        /// </summary>
        /// <returns></returns>
        Task<string> JwtTokenGeneration();
    }
}