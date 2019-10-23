using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.ViewModels
{
    /// <summary>
    /// Viewmodel containing a list of people and errormessage.
    /// </summary>
    public class PersonListWithMessage
    {
        public List<Person> People { get; set; } = new List<Person>();

        /// <summary>
        /// Used in correlation with <see cref="StatusMessages"/> and/or <see cref="ActionMessages"/>.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Viewmodel containing one person and errormessage.
    /// </summary>
    public class PersonWithMessage
    {
        public Person Person { get; set; }

        /// <summary>
        /// Used in correlation with <see cref="StatusMessages"/> and/or <see cref="ActionMessages"/>.
        /// </summary>
        public string Message { get; set; }
    }
}