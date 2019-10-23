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
        public const string NotFound = "Unexpected error: No object with given ID was found.";
    }

    /// <summary>
    /// The class used when an action was successful.
    /// </summary>
    public class ActionMessages
    {
        /// <summary>
        /// Used when a requested object has been successfully created.
        /// </summary>
        public const string Created = "The object was successfully created.";

        /// <summary>
        /// Used when a requested object has been successfully updated.
        /// </summary>
        public const string Updated = "The object was successfully updated.";

        /// <summary>
        /// Used when a requested object has been successfully deleted.
        /// </summary>
        public const string Deleted = "The object was successfully deleted.";

        /// <summary>
        /// Used when a requested object has been successfully found.
        /// </summary>
        public const string Found = "The object was successfully found.";
    }
}