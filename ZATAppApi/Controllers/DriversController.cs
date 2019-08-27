using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using ZATApp.ViewModels;
using PagedList;
using ZATApp.Common;
using ZATApp.Common.Functions;

namespace ZATAppApi.Controllers
{
    public class DriversController : Controller
    {
        /// <summary>
        /// Index method of Drivers controller which will return a list of drivers registered with the system
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {
            try
            {
                List<ViewDriversViewModel> lstDrivers = new List<ViewDriversViewModel>();
                foreach (var item in Driver.GetAllDrivers())
                {
                    var vehicle = item.GetVehicle();
                    lstDrivers.Add(new ViewDriversViewModel
                    {
                        CNIC = item.CNIC_Number,
                        ContactNumber = item.ContactNumber.LocalFormatedPhoneNumber,
                        Id = item.UserId,
                        Name = item.FullName.FirstName + " " + item.FullName.LastName,
                        VehicleType = vehicle.Type.Name,
                        IsCleared = item.IsCleared,
                        Balance = item.Balance
                    });
                }
                PagedList<ViewDriversViewModel> model = new PagedList<ViewDriversViewModel>(lstDrivers, page ?? 1, Constants.PAGGING_RANGE);
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to return the Details about a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewDetails(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                var vehicle = driver.GetVehicle();
                DriverDetailsViewModel model = new DriverDetailsViewModel
                {
                    Balance = driver.Balance,
                    CNIC = driver.CNIC_Number,
                    ContactNumber = driver.ContactNumber.LocalFormatedPhoneNumber,
                    CreditLimit = driver.CreditLimit,
                    Id = driver.UserId,
                    IsBlocked = driver.IsBlocked,
                    Name = driver.FullName.FirstName + " " + driver.FullName.LastName,
                    Rating = decimal.Round(driver.TotalRating, 2),
                    RegisterationNumber = vehicle.RegisterationNumber.FormattedNumber,
                    RidesCompleted = driver.GetCompletedRides().Count,
                    VehcileType = vehicle.Type.Name,
                    VehicleModel = vehicle.Model
                };
                model.Comments = new List<ZATApp.Models.Common.RatingAndComments>();
                foreach (var item in driver.GetRatingAndComments())
                {
                    model.Comments.Add(new ZATApp.Models.Common.RatingAndComments
                    {
                        Comment = item.Comment,
                        Rating = item.Rating,
                        Rider = item.Rider
                    });
                }
                model.ManualTransactions = new List<ManualTransactionViewModel>(50);
                foreach (var item in driver.GetManualTransactions())
                {
                    model.ManualTransactions.Add(new ManualTransactionViewModel
                    {
                        Amount = decimal.Round(item.Amount, 2),
                        Time = UISupportiveFunctions.GetPassedTimeSpanFromNow(item.TransactionDateTime)
                    });
                }
                model.MobileTransactions = new List<MobileTransactionsViewModel>();
                foreach (var item in driver.GetAllMobileAccountTransactions())
                {
                    if (item.IsVerified)
                    {
                        model.MobileTransactions.Add(new MobileTransactionsViewModel
                        {
                            Amount = decimal.Round(item.Amount, 2),
                            IsVerified = item.IsVerified,
                            ReferenceNumber = item.ReferenceNumber,
                            ServiceName = item.MobileAccountServiceProviderName,
                            Time = UISupportiveFunctions.GetPassedTimeSpanFromNow(item.TransactionRegisteredTime)
                        });
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to block a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Block(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                driver.IsBlocked = true;
                return RedirectToAction("ViewDetails", id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}