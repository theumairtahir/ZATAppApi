using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.Models;
using System.ComponentModel.DataAnnotations;
using ZATAppApi.ViewModels;

namespace ZATAppApi.Controllers
{
    /// <summary>
    /// Manages the home tasks for the user
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Controller Starts from this action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard");
            }
            else if(User.IsInRole("SubAdmin"))
            {
                return RedirectToAction("BookRide", "Rides");
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Action to show statistics for the Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
