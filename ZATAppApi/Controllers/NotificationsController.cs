using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using ZATApp.Common;
using ZATApp.Common.Functions;
using PagedList;
using ZATApp.Models.Exceptions;
using ZATApp.Models.Common;
using ZATApp.ViewModels;

namespace ZATAppApi.Controllers
{
    public class NotificationsController : Controller
    {
        /// <summary>
        /// Index method to show list of notifications sent
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {

            try
            {
                List<SmsViewModel> lstSms = new List<SmsViewModel>();
                foreach (var item in Sms.GetAllSms())
                {
                    lstSms.Add(new SmsViewModel
                    {
                        Body = item.Body,
                        Id = item.SmsId,
                        Time = UISupportiveFunctions.GetPassedTimeSpanFromNow(item.SentDateTime)
                    });
                }
                PagedList<SmsViewModel> model = new PagedList<SmsViewModel>(lstSms, page ?? 1, Constants.PAGGING_RANGE);
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Returns a view contains a list of receviers of a sms
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ViewReceivers(long id, int? page)
        {
            try
            {
                Sms sms = new Sms(id);
                List<SmsReceiversViewModel> lstReceivers = new List<SmsReceiversViewModel>();
                foreach (var item in sms.GetReceivers())
                {
                    lstReceivers.Add(new SmsReceiversViewModel
                    {
                        Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                        Id = item.UserId,
                        Name = item.FullName.FirstName + " " + item.FullName.LastName
                    });
                }
                PagedList<SmsReceiversViewModel> model = new PagedList<SmsReceiversViewModel>(lstReceivers, page ?? 1, Constants.PAGGING_RANGE);
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}