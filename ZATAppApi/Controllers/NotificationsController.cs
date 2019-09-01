using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.Models;
using ZATAppApi.Common;
using ZATAppApi.Common.Functions;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Models.Common;
using ZATAppApi.ViewModels;

namespace ZATAppApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NotificationsController : Controller
    {
        /// <summary>
        /// Index method to show list of notifications sent
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            try
            {
                List<SmsViewModel> model = new List<SmsViewModel>();
                foreach (var item in Sms.GetAllSms())
                {
                    model.Add(new SmsViewModel
                    {
                        Body = item.Body,
                        Id = item.SmsId,
                        Time = UISupportiveFunctions.GetPassedTimeSpanFromNow(item.SentDateTime)
                    });
                }
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
        /// <returns></returns>
        public ActionResult ViewReceivers(long id)
        {
            try
            {
                Sms sms = new Sms(id);
                List<SmsReceiversViewModel> model = new List<SmsReceiversViewModel>();
                foreach (var item in sms.GetReceivers())
                {
                    model.Add(new SmsReceiversViewModel
                    {
                        Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                        Id = item.UserId,
                        Name = item.FullName.FirstName + " " + item.FullName.LastName
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to forward an old sms
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ForwardSms(long id)
        {
            return RedirectToAction("SendNotification", new { id = id });
        }
        /// <summary>
        /// Action to send an sms 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SendNotification(long? id)
        {
            if (id == null)
            {
                SendSmsViewModel model = new SendSmsViewModel
                {
                    MessageId = 0
                };
                return View(model);
            }
            else
            {
                try
                {
                    Sms sms = new Sms(id ?? 0);
                    SendSmsViewModel model = new SendSmsViewModel
                    {
                        Body = sms.Body,
                        MessageId = sms.SmsId
                    };
                    return View("SendNotification", model);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ErrorPage", "Error", ex);
                }
            }
        }
        /// <summary>
        /// Method to receive the POST Method call of Send Notification form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotification(SendSmsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                Sms sms;
                if (model.MessageId != 0)
                {
                    sms = new Sms(model.MessageId);
                    if (sms.Body != model.Body)
                    {
                        sms = new Sms(DateTime.Now, model.Body);
                    }
                }
                else
                {
                    sms = new Sms(DateTime.Now, model.Body);
                }

                if (model.Receiver == SendSmsViewModel.Receivers.All)
                {
                    foreach (var item in ZATAppApi.Models.User.GetAllUsers())
                    {
                        SendSMS(sms.Body, item.ContactNumber.PhoneNumberFormat);
                        item.SendSms(sms);
                    }
                }
                else if (model.Receiver == SendSmsViewModel.Receivers.Drivers)
                {
                    foreach (var item in Driver.GetAllDrivers())
                    {
                        SendSMS(model.Body, item.ContactNumber.PhoneNumberFormat);
                        item.SendSms(sms);
                    }
                }
                else if (model.Receiver == SendSmsViewModel.Receivers.Riders)
                {
                    foreach (var item in Rider.GetAllRiders())
                    {
                        SendSMS(model.Body, item.ContactNumber.PhoneNumberFormat);
                        item.SendSms(sms);
                    }
                }
                else if (model.Receiver == SendSmsViewModel.Receivers.SubAdmin)
                {
                    foreach (var item in SubAdmin.GetAllSubAdmins())
                    {
                        SendSMS(model.Body, item.ContactNumber.PhoneNumberFormat);
                        item.SendSms(sms);
                    }
                }
                return View("Confirmation");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Shows the list of drivers who have payment due to the service provider
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDriversWithDues()
        {
            List<DriversWithDuesViewModel> model = new List<DriversWithDuesViewModel>();
            foreach (var item in Driver.GetAllDrivers())
            {
                if (item.Balance < 0)
                {
                    model.Add(new DriversWithDuesViewModel
                    {
                        AmountDue = decimal.Round(Math.Abs(item.Balance), 2),
                        Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                        FirstName = item.FullName.FirstName,
                        LastName = item.FullName.LastName,
                        Id = item.UserId
                    });
                }
            }
            return View(model);
        }
        /// <summary>
        /// Action to send due notifications ot all drivers
        /// </summary>
        /// <returns></returns>
        public ActionResult SendDueNotificationToAll()
        {
            try
            {
                foreach (var item in Driver.GetAllDrivers())
                {
                    if (item.Balance < 0)
                    {
                        string messageBody = String.Format(Constants.DUE_PAYMENT_NOTIFICATION_FORMAT, item.FullName.FirstName, decimal.Round(Math.Abs(item.Balance), 2));
                        SendSMS(messageBody, item.ContactNumber.PhoneNumberFormat);
                    }
                }
                return View("Confirmation");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to send due notifications to a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SendDueNotification(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                string messageBody = String.Format(Constants.DUE_PAYMENT_NOTIFICATION_FORMAT, driver.FullName.FirstName, decimal.Round(Math.Abs(driver.Balance), 2));
                SendSMS(messageBody, driver.ContactNumber.PhoneNumberFormat);
                return View("Confirmation");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Page", ex);
            }
        }
        private void SendSMS(string body, string contact)
        {
            //This method will use to a sms sending service to be implemented later
        }
    }
}