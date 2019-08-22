using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;

namespace ZATApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        /// <summary>
        /// Method to be called by using AJAX to getting UI updated
        /// </summary>
        /// <returns></returns>
        public int GetUnverifiedTransactionsCount()
        {
            int i = 0;
            try
            {
                i = MobileAccountTransactionLog.GetAllUnverifiedMobileAccountTransactions().Count;
            }
            catch (Exception)
            {
                
            }
            return i;
        }
    }
}
