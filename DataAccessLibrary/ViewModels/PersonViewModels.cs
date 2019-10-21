using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.ViewModels
{
    /// <summary>
    /// Viewmodel containing a list of people.
    /// </summary>
    public class PersonList
    {
        public List<Person> People { get; set; } = new List<Person>();
    }
}