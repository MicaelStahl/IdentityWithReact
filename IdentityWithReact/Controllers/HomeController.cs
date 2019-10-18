using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityWithReact.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;

        public HomeController(ILogger<HomeController> log)
        {
            _log = log;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method used for errors occuring throughout the application.
        /// </summary>
        /// <param name="error">Parameter that explains the error.</param>
        /// <param name="exception">Parameter that indicates what went wrong.</param>
        /// <returns></returns>
        public IActionResult Error(string error, Exception exception)
        {
            _log.LogError(exception, error);

            ViewBag.exceptionMessage = exception.Message;

            return View(new ErrorViewModel { ErrorMessage = error, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}