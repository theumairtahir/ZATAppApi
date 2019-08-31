using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.Models;
using ZATAppApi.Common;
using ZATAppApi.ViewModels;

namespace ZATAppApi.Controllers
{
    public class RidersController : Controller
    {
        /// <summary>
        /// Main Action of the riders controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                List<ViewRidersViewModel> model = new List<ViewRidersViewModel>();
                foreach (var item in Rider.GetAllRiders())
                {
                    model.Add(new ViewRidersViewModel
                    {
                        Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                        Name = item.FullName.FirstName + " " + item.FullName.LastName,
                        Id = item.UserId
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
        /// Action to return details of a rider
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        public ActionResult ViewDetails(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                var temp = rider.GetCompletedRides();
                List<Ride> lstTop5CompletedRides;
                if (temp.Count > 5)
                {
                    lstTop5CompletedRides = rider.GetCompletedRides().GetRange(0, 5);
                }
                else
                {
                    lstTop5CompletedRides = rider.GetCompletedRides().GetRange(0, temp.Count);
                }
                ViewRiderDetailsViewModel model = new ViewRiderDetailsViewModel
                {
                    CompletedRides = temp.Count,
                    Contact = rider.ContactNumber.LocalFormatedPhoneNumber,
                    Name = rider.FullName.FirstName + " " + rider.FullName.LastName,
                    IsActive = rider.IsActive,
                    Rides = new List<ViewRiderDetailsViewModel.RideDetailsViewModel>(),
                    Id = rider.UserId,
                    IsBlocked = rider.IsBlocked
                };
                foreach (var item in lstTop5CompletedRides)
                {
                    model.Rides.Add(new ViewRiderDetailsViewModel.RideDetailsViewModel
                    {
                        EndPoint = item.Destination,
                        StartPoint = item.PickUpLocation,
                        Route = item.Route,
                        EndTime = item.DropOffTime.ToString("dd-mm-yyyy hh:mm tt"),
                        StartTime = item.PickUpTime.ToString("dd-mm-yyyy hh:mm tt"),
                        AmountPaid = decimal.Round(item.GetPaymentSummary().GTotal)
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        public ActionResult Edit(long id)
        {
            ViewBag.ErrorFlag = false;
            try
            {
                Rider rider = new Rider(id);
                EditRiderViewModel model = new EditRiderViewModel
                {
                    CompanyCode = rider.ContactNumber.CompanyCode,
                    CountryCode = rider.ContactNumber.CountryCode,
                    FirstName = rider.FullName.FirstName,
                    Id = rider.UserId,
                    LastName = rider.FullName.LastName,
                    Number = rider.ContactNumber.PhoneNumber
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRiderViewModel model)
        {
            ViewBag.ErrorFlag = false;
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                Rider rider = new Rider(model.Id);
                rider.FullName = new User.NameFormat
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                rider.ContactNumber = new User.ContactNumberFormat(model.CountryCode, model.CompanyCode, model.Number);
                return RedirectToAction("ViewDetails", new { id = model.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to block a Rider
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Block(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                rider.IsBlocked = true;
                return RedirectToAction("ViewDetails", new { id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to unblock a rider
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Unblock(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                rider.IsBlocked = false;
                return RedirectToAction("ViewDetails", new { id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}