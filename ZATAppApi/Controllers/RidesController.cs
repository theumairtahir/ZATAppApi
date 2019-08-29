using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using ZATApp.Models.Exceptions;
using PagedList;
using ZATApp.Common.Functions;
using ZATApp.ViewModels;
using static ZATApp.ViewModels.ViewFaresViewModel;

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
    }
}