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
        public ActionResult SetFare()
        {
            return View();
        }
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
        public ActionResult ViewFares()
        {
            ViewFaresViewModel model = new ViewFaresViewModel();
            foreach (var item in VehicleType.GetAllVehicleTypes())
            {
                Fare fare = item.GetCurrentFare();
                model.CurrentFareInfo.Add(new FareInfo
                {
                    DateOfInclusion = UISupportiveFunctions.GetPassedDateSpanFromNow(fare.Date),
                    DistanceTravelledPerKmFee = fare.DistanceTravelledPerKm,
                    DropOffFee = fare.DropOffFare,
                    Gst = fare.GSTPercent,
                    PickUpFee = fare.PickUpFare,
                    ServiceCharges = fare.ServiceChargesPercent,
                    VehicleType = item.Name
                });

            }
            foreach (var fareHistory in Fare.GetAllFares())
            {
                model.UpdationHistory.Add(new FareInfo
                {
                    DateOfInclusion = UISupportiveFunctions.GetPassedDateSpanFromNow(fareHistory.Date),
                    DistanceTravelledPerKmFee = fareHistory.DistanceTravelledPerKm,
                    DropOffFee = fareHistory.DropOffFare,
                    Gst = fareHistory.GSTPercent,
                    PickUpFee = fareHistory.PickUpFare,
                    ServiceCharges = fareHistory.ServiceChargesPercent,
                    VehicleType = fareHistory.VehicleType.Name
                });
            }
            return View(model);
        }
    }
}