using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.Models;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Common.Functions;
using ZATAppApi.ViewModels;
using static ZATAppApi.ViewModels.ViewFaresViewModel;

namespace ZATAppApi.Controllers
{
    public class RidesController : Controller
    {
        /// <summary>
        /// Action returns a view to set fare for a vehicle type
        /// </summary>
        /// <returns></returns>
        public ActionResult SetFare()
        {
            return View();
        }
        /// <summary>
        /// Post method to be called on form submission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetFare(SetFareViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                VehicleType vehicleType = new VehicleType(model.VehicleTypeId);
                vehicleType.UpdateFare(new VehicleType.FareInfo
                {
                    DateOfInclusion = DateTime.Now,
                    DistanceTravelledPerKmFee = model.DistanceTravelledPerKmCharges,
                    DropOffFee = model.DropOffFee,
                    Gst = model.GST,
                    PickUpFee = model.PickUpFee,
                    ServiceCharges = model.ServiceCharges
                });
                return RedirectToAction("ViewFares");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// View detailed history of fare updations including current fare
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewFares()
        {
            try
            {
                ViewFaresViewModel model = new ViewFaresViewModel();
                foreach (var item in VehicleType.GetAllVehicleTypes())
                {
                    Fare fare = item.GetCurrentFare();
                    model.CurrentFareInfo.Add(new FareInfo
                    {
                        DateOfInclusion = UISupportiveFunctions.GetPassedDateSpanFromNow(fare.Date),
                        DistanceTravelledPerKmFee = decimal.Round(fare.DistanceTravelledPerKm, 2),
                        DropOffFee = decimal.Round(fare.DropOffFare, 2),
                        Gst = fare.GSTPercent,
                        PickUpFee = decimal.Round(fare.PickUpFare, 2),
                        ServiceCharges = fare.ServiceChargesPercent,
                        VehicleType = item.Name
                    });

                }
                foreach (var fareHistory in Fare.GetAllFares())
                {
                    model.UpdationHistory.Add(new FareInfo
                    {
                        DateOfInclusion = UISupportiveFunctions.GetPassedDateSpanFromNow(fareHistory.Date),
                        DistanceTravelledPerKmFee = decimal.Round(fareHistory.DistanceTravelledPerKm, 2),
                        DropOffFee = decimal.Round(fareHistory.DropOffFare, 2),
                        Gst = fareHistory.GSTPercent,
                        PickUpFee = decimal.Round(fareHistory.PickUpFare, 2),
                        ServiceCharges = fareHistory.ServiceChargesPercent,
                        VehicleType = fareHistory.VehicleType.Name
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
        /// Action to show a list of promo codes
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPromos()
        {
            try
            {
                List<PromoCodeViewModel> model = new List<PromoCodeViewModel>();
                foreach (var item in PromoCode.GetAllPromoCodes())
                {
                    model.Add(new PromoCodeViewModel
                    {
                        Discount = item.DiscountPercent,
                        Code = item.Code,
                        IsOpen = item.IsOpen
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        public ActionResult AddPromo()
        {
            ViewBag.ErrorFlag = false;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPromo(PromoCodeViewModel model)
        {
            ViewBag.ErrorFlag = false;
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                try
                {
                    PromoCode promo = new PromoCode(model.Code, model.Discount);
                    return RedirectToAction("ViewPromos");
                }
                catch (UniqueKeyViolationException ex)
                {
                    ViewBag.ErrorFlag = true;
                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View();
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to open a closed promo, to be called via AJAX
        /// </summary>
        public ActionResult OpenPromo(string code)
        {
            try
            {
                PromoCode promo = PromoCode.GetPromoCode(code);
                promo.IsOpen = true;
                return RedirectToAction("ViewPromos");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to close an open promo
        /// </summary>
        public ActionResult ClosePromo(string code)
        {
            try
            {
                PromoCode promo = PromoCode.GetPromoCode(code);
                promo.IsOpen = false;
                return RedirectToAction("ViewPromos");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}