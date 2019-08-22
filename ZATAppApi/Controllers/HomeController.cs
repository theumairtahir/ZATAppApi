using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using PagedList;
using PagedList.Mvc;
using System.ComponentModel.DataAnnotations;
using ZATApp.ViewModels;

namespace ZATApp.Controllers
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
        /// <summary>
        /// Method to show a list of unverified transactions in the system
        /// </summary>
        /// <param name="page">Page Number us to do paging</param>
        /// <returns></returns>
        public ActionResult ViewUnverifiedTransactions(int? page)
        {
            try
            {
                List<ViewUnverifiedTransactionsViewModel> lstTransactions = new List<ViewUnverifiedTransactionsViewModel>();
                foreach (var item in MobileAccountTransactionLog.GetAllUnverifiedMobileAccountTransactions())
                {
                    lstTransactions.Add(new ViewUnverifiedTransactionsViewModel
                    {
                        Id=item.TransactionId,
                        Amount = item.Amount,
                        DriverName = item.Driver.FullName.FirstName + " " + item.Driver.FullName.LastName,
                        IsVerified = item.IsVerified,
                        ReferenceNumber = item.ReferenceNumber,
                        ServiceName = item.MobileAccountServiceProviderName,
                        Time = item.TransactionRegisteredTime.ToString("dd-mmm-yyyy hh:mm:ss")
                    });
                }
                PagedList<ViewUnverifiedTransactionsViewModel> model = new PagedList<ViewUnverifiedTransactionsViewModel>(lstTransactions, page ?? 1, 50);
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        [HttpGet]
        public void VerifyTransaction(long id)
        {
            try
            {
                MobileAccountTransactionLog log = new MobileAccountTransactionLog(id);
                log.IsVerified = true;
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
