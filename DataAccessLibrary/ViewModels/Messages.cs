using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.ViewModels
{
    /// <summary>
    /// The class used for error descriptions.
    /// </summary>
    public class StatusMessages
    {
        private static Guid Id { get; set; }

        public StatusMessages(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Used when a wanted ID was blank.
        /// <code>Ex: (id == Guid.Empty)</code>
        /// </summary>
        public const string EmptyId = "Unexpected error: The given ID was blank.";

        /// <summary>
        /// Used when a database call returns a null or empty list.
        /// <code>var objects = await _db.dbObjects.ToListAsync()</code>
        /// <code>objects == null || objects.Count == 0</code>
        /// </summary>
        public const string EmptyList = "Unexpected error: No objects could be found in database.";

        /// <summary>
        /// Used when a recieved model or input contains one or more invalid fields.
        /// <code>string invalidString = ""</code>
        /// </summary>
        public const string InvalidFields = "Unexpected error: One or more fields were invalid.";

        /// <summary>
        /// Used when a requested object wasn't found.
        /// <code>Ex: (Person person = await _db.People.FirstOrDefault(x=>x.Id == id) == null)</code>
        /// </summary>
        public readonly string NotFound = $"Unexpected error: No object with ID: {Id} was found.";
    }

    /// <summary>
    /// The class used when an action was successful.
    /// </summary>
    public class ActionMessages
    {
        private static string Str { get; set; }

        public ActionMessages(string str)
        {
            Str = str;
        }

        /// <summary>
        /// Used when a requested object has been successfully created.
        /// </summary>
        public readonly string Created = $"The {Str} was successfully created.";

        /// <summary>
        /// Used when a requested object has been successfully updated.
        /// </summary>
        public readonly string Updated = $"The {Str} was successfully updated.";

        /// <summary>
        /// Used when a requested object has been successfully deleted.
        /// </summary>
        public readonly string Deleted = $"The {Str} was successfully deleted.";

        /// <summary>
        /// Used when a requested object has been successfully found.
        /// </summary>
        public readonly string Found = $"The {Str} was successfully found.";
    }
}