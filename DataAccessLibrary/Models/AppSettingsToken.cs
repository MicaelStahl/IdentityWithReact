using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    /// <summary>
    /// Contains a secret used for token generation.
    /// </summary>
    public class AppSettingsToken
    {
        /// <summary>
        /// The secret for token generation.
        /// </summary>
        public string Secret { get; set; }
    }
}