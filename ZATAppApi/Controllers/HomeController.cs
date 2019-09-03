using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Mvc;
using ZATAppApi.Models;
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
            else if (User.IsInRole("SubAdmin"))
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
        /// <summary>
        /// Method to be called via AJAX to load form data
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public string WeeklyCompletedRides()
        {
            string s = "";
            try
            {
                List<Ride> lstRides = Ride.GetTotalCompletedRides(DateTime.Now.AddMonths(-1));
                List<WeeklyCompletedRidesViewModel> model = new List<WeeklyCompletedRidesViewModel>();
                List<Ride> week1 = new List<Ride>();
                List<Ride> week2 = new List<Ride>();
                List<Ride> week3 = new List<Ride>();
                List<Ride> week4 = new List<Ride>();
                foreach (var item in lstRides)
                {
                    var date = item.DropOffTime.Date;
                    DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
                    DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
                    if (firstMonthMonday > date)
                    {
                        firstMonthDay = firstMonthDay.AddMonths(-1);
                        firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
                    }
                    var week = (date - firstMonthMonday).Days / 7 + 1;
                    if (week == 1)
                    {
                        week1.Add(item);
                    }
                    else if (week == 2)
                    {
                        week2.Add(item);
                    }
                    else if (week == 3)
                    {
                        week3.Add(item);
                    }
                    else
                    {
                        week4.Add(item);
                    }
                }
                if (week1.Count > 0)
                {
                    var monCount = 0;
                    var tueCount = 0;
                    var wedCount = 0;
                    var thurCount = 0;
                    var friCount = 0;
                    var satCount = 0;
                    var sunCount = 0;
                    foreach (var item in week1)
                    {
                        if (item.DropOffTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            monCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            tueCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            wedCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            thurCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            friCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sunCount++;
                        }
                    }
                    model.Add(new WeeklyCompletedRidesViewModel
                    {
                        linecolor = "#9e17b3",
                        title = "Week 1",
                        values = new List<GraphCordinate>() {
                            new GraphCordinate { X = "Monday", Y = monCount },
                            new GraphCordinate { X = "Tuesday", Y = tueCount },
                            new GraphCordinate { X = "Wednesday", Y = wedCount },
                            new GraphCordinate { X = "Thursday", Y = thurCount },
                            new GraphCordinate { X = "Friday", Y = friCount },
                            new GraphCordinate { X = "Saturday", Y = satCount },
                            new GraphCordinate { X = "Sunday", Y = sunCount }
                        }
                    });
                }
                if (week2.Count > 0)
                {
                    var monCount = 0;
                    var tueCount = 0;
                    var wedCount = 0;
                    var thurCount = 0;
                    var friCount = 0;
                    var satCount = 0;
                    var sunCount = 0;
                    foreach (var item in week2)
                    {
                        if (item.DropOffTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            monCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            tueCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            wedCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            thurCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            friCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sunCount++;
                        }
                    }
                    model.Add(new WeeklyCompletedRidesViewModel
                    {
                        linecolor = "#c9811a",
                        title = "Week 2",
                        values = new List<GraphCordinate>() {
                            new GraphCordinate { X = "Monday", Y = monCount },
                            new GraphCordinate { X = "Tuesday", Y = tueCount },
                            new GraphCordinate { X = "Wednesday", Y = wedCount },
                            new GraphCordinate { X = "Thursday", Y = thurCount },
                            new GraphCordinate { X = "Friday", Y = friCount },
                            new GraphCordinate { X = "Saturday", Y = satCount },
                            new GraphCordinate { X = "Sunday", Y = sunCount }
                        }
                    });
                }
                if (week3.Count > 0)
                {
                    var monCount = 0;
                    var tueCount = 0;
                    var wedCount = 0;
                    var thurCount = 0;
                    var friCount = 0;
                    var satCount = 0;
                    var sunCount = 0;
                    foreach (var item in week3)
                    {
                        if (item.DropOffTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            monCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            tueCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            wedCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            thurCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            friCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sunCount++;
                        }
                    }
                    model.Add(new WeeklyCompletedRidesViewModel
                    {
                        linecolor = "#b31772",
                        title = "Week 3",
                        values = new List<GraphCordinate>() {
                            new GraphCordinate { X = "Monday", Y = monCount },
                            new GraphCordinate { X = "Tuesday", Y = tueCount },
                            new GraphCordinate { X = "Wednesday", Y = wedCount },
                            new GraphCordinate { X = "Thursday", Y = thurCount },
                            new GraphCordinate { X = "Friday", Y = friCount },
                            new GraphCordinate { X = "Saturday", Y = satCount },
                            new GraphCordinate { X = "Sunday", Y = sunCount }
                        }
                    });
                }
                if (week4.Count > 0)
                {
                    var monCount = 0;
                    var tueCount = 0;
                    var wedCount = 0;
                    var thurCount = 0;
                    var friCount = 0;
                    var satCount = 0;
                    var sunCount = 0;
                    foreach (var item in week4)
                    {
                        if (item.DropOffTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            monCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            tueCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            wedCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            thurCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            friCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satCount++;
                        }
                        else if (item.DropOffTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sunCount++;
                        }
                    }
                    model.Add(new WeeklyCompletedRidesViewModel
                    {
                        linecolor = "#17b399",
                        title = "Week 4",
                        values = new List<GraphCordinate>() {
                            new GraphCordinate { X = "Monday", Y = monCount },
                            new GraphCordinate { X = "Tuesday", Y = tueCount },
                            new GraphCordinate { X = "Wednesday", Y = wedCount },
                            new GraphCordinate { X = "Thursday", Y = thurCount },
                            new GraphCordinate { X = "Friday", Y = friCount },
                            new GraphCordinate { X = "Saturday", Y = satCount },
                            new GraphCordinate { X = "Sunday", Y = sunCount }
                        }
                    });
                }
                var json = JsonConvert.SerializeObject(model);
                s = json.ToString();
            }
            catch (Exception)
            {

            }
            return s;
        }
        /// <summary>
        /// Method to be called via AJAX to get completed rides count
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public long GetCompletedRidesCount()
        {
            long count = 0;
            try
            {
                count = Ride.GetTotalCompletedRides();
            }
            catch (Exception)
            {

            }
            return count;
        }
        /// <summary>
        /// Method to be called via AJAX to get registered drivers count
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public long GetRegisteredDriversCount()
        {
            long count = 0;
            try
            {
                count = Driver.GetAllDrivers().Count;
            }
            catch (Exception)
            {

            }
            return count;
        }
        /// <summary>
        /// Method to be called via AJAX to get registered riders count
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public long GetRegisteredRidersCount()
        {
            long count = 0;
            try
            {
                count = Rider.GetAllRiders().Count;
            }
            catch (Exception)
            {

            }
            return count;
        }
        /// <summary>
        /// Method to be called via AJAX to get Admin balance
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public decimal GetTotalBalance()
        {
            decimal balance = 0;
            try
            {
                balance = AccountingLog.GetAdminBalance();
            }
            catch (Exception)
            {

            }
            return decimal.Round(balance, 0);
        }
        /// <summary>
        /// Method to be called via AJAX to get number of active rides
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public int GetActiveRidesCount()
        {
            int count = 0;
            try
            {
                count = Ride.GetActiveRides().Count;
            }
            catch (Exception)
            {

            }
            return count;
        }
        /// <summary>
        /// Method to be called via AJAX to get number of active rides
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public string AccountsData()
        {
            string s = "";
            try
            {
                AccountsGraphViewModel.Dataset debit = new AccountsGraphViewModel.Dataset()
                {

                    backgroundColor = "#b31717",
                    borderWidth = 1,
                    label = "Debit",
                    borderColor = "#b31717",
                };
                AccountsGraphViewModel.Dataset credit = new AccountsGraphViewModel.Dataset()
                {
                    backgroundColor = "#21b317",
                    borderWidth = 1,
                    label = "Credit",
                    borderColor = "#21b317"
                };
                for (int i = 1; i <= 12; i++)
                {

                    try
                    {
                        var balance = AccountingLog.GetAdminBalanceByMonth(new DateTime(DateTime.Now.Year, i, 1));
                        if (balance > 0)
                        {
                            credit.data.Add(balance);
                            debit.data.Add(0);
                        }
                        else
                        {
                            debit.data.Add(balance);
                            credit.data.Add(0);
                        }
                    }
                    catch (Exception)
                    {

                        debit.data.Add(0);
                        credit.data.Add(0);
                    }
                }
                AccountsGraphViewModel model = new AccountsGraphViewModel();
                model.datasets[0] = debit;
                model.datasets[1] = credit;
                var json = JsonConvert.SerializeObject(model);
                s = json.ToString();
            }
            catch (Exception)
            {

            }
            return s;
        }
    }
}
