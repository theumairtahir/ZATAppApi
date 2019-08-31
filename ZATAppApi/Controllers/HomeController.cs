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
    public class HomeController : Controller
    {
        /// <summary>
        /// Controller Starts from this action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        
    }
}
