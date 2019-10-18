using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.ViewModels
{
    /// <summary>
    /// Viewmodel
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; }
    }
}