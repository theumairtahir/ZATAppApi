using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.Models;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Common.Functions;
using ZATAppApi.ViewModels;
using Newtonsoft.Json.Linq;
using static ZATAppApi.ViewModels.ViewFaresViewModel;
using System.Net.Http;
using ZATAppApi.Common;
using ZATAppApi.Models.Common;

namespace ZATAppApi.Controllers
{
    public class RidesController : Controller
    {
        /// <summary>
        /// Action returns a view to set fare for a vehicle type
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult SetFare()
        {
            return View();
        }
        /// <summary>
        /// Post method to be called on form submission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Action to Add Discount promos to the system
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult AddPromo()
        {
            ViewBag.ErrorFlag = false;
            return View();
        }
        /// <summary>
        /// POST Method to be called on form submission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Action to be used to Book a Ride
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        public ActionResult BookRide()
        {
            return View();
        }
        /// <summary>
        /// Action to choose the destination point
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        public ActionResult Destination(decimal lat, decimal lng)
        {
            try
            {
                Location pickupPoint = new Location
                {
                    Latitude = lat,
                    Longitude = lng
                };
                ViewBag.PickUpPoint = pickupPoint;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to enter the rider details after choosion pickup and destination points
        /// </summary>
        /// <param name="lat1">Pickup latitude</param>
        /// <param name="lat2">Destination latitude</param>
        /// <param name="lng1">Pickup Longitude</param>
        /// <param name="lng2">Destination Logitude</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        public ActionResult RiderDetails(decimal lat1, decimal lat2, decimal lng1, decimal lng2)
        {
            BookRideViewModel model = new BookRideViewModel
            {
                PLat = lat1,
                Plng = lng1,
                DLat = lat2,
                Dlng = lat2
            };
            return View(model);
        }
        /// <summary>
        /// POST Method for the form submission and end of the ride booking process
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RiderDetails(BookRideViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //create rider
                Rider rider = new Rider(new User.NameFormat { FirstName = model.RiderFirstName, LastName = model.RidierLastName }, new User.ContactNumberFormat(model.CountryCode, model.CompanyCode, model.Number));
                //Book Ride
                var ride = rider.BookRide(new Ride.RideBookingDetails
                {
                    Destination = new Location { Latitude = model.DLat, Longitude = model.Dlng },
                    PickUpLocation = new Location { Latitude = model.PLat, Longitude = model.Plng },
                    VehicleType = new VehicleType(model.VehicleType)
                });
                //Add Promo
                if (model.PromoCode != null || model.PromoCode == String.Empty)
                {
                    ride.AddPromo(PromoCode.GetPromoCode(model.PromoCode));
                }
                return RedirectToAction("BookRide");
            }
            catch (UniqueKeyViolationException ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View(model);
            }
            catch (UnsuccessfullProcessException)
            {
                ModelState.AddModelError(String.Empty, "Seems like no driver is available near the pick-up point.");
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Method to return the number of drivers present at the nearby location
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        public int GetNearbyDriversCount(decimal lat, decimal lng)
        {
            int count = 0;
            try
            {
                Location location = new Location
                {
                    Latitude = lat,
                    Longitude = lat
                };
                foreach (var item in Driver.GetAllDrivers())
                {
                    if (location.DistanceToAPoint(item.LastLocation) < Constants.DEFAULT_DISTANCE_RADIUS)
                    {
                        count++;
                    }
                }
            }
            catch (Exception)
            {

            }
            return count;
        }
        /// <summary>
        /// Returns the places on the basis of search
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        [HttpGet]
        public async System.Threading.Tasks.Task<string> GetPickupPlaces(string q)
        {
            string rowFormat = "<tr>" +
                            "<td> {0} {1}</td>" +

                               "<td><a href ='/Rides/Destination?lat={2}&lng={3}' class='btn btn-primary' title='Set Pick-Up Point'><span class='fa fa-map-pin'></span></a></td>" +
                        "</tr>";

            string html = "";
            List<string> lstRows = new List<string>();
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=" + q + "&inputtype=textquery&fields=formatted_address,name,geometry&key=" + Constants.GOOGLE_API_KEY;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject googleResponse = JObject.Parse(content);
                    JArray googlePlaces = (JArray)googleResponse["candidates"];
                    foreach (var item in googlePlaces)
                    {
                        string name = (string)item["name"];
                        string address = (string)item["formatted_address"];
                        JObject location = (JObject)item["geometry"]["location"];
                        string lat = (string)location["lat"];
                        string lng = (string)location["lng"];
                        lstRows.Add(String.Format(rowFormat, name, address, lat, lng));
                    }
                }
                else
                {
                    lstRows.Add(String.Format(rowFormat, "No Places are found", "Please write name correctly", String.Empty, String.Empty));
                }
            }
            catch (Exception)
            {
                lstRows.Add(String.Format(rowFormat, "No Places are found", "Please write name correctly", String.Empty, String.Empty));
            }
            html = String.Join("\n", lstRows);
            return html;
        }
        /// <summary>
        /// return the places for the destination
        /// </summary>
        /// <param name="q"></param>
        /// <param name="lat1">Pickup Latitude</param>
        /// <param name="lng1">Pickup Longitude</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SubAdmin")]
        [HttpGet]
        public async System.Threading.Tasks.Task<string> GetDestinationPlaces(string q, decimal lat1, decimal lng1)
        {
            string rowFormat = "<tr>" +
                            "<td> {0} {1}</td>" +

                               "<td><a href ='/Rides/RiderDetails?lat1=" + lat1 + "&lng1=" + lng1 + "&lat2={2}&lng2={3}' class='btn btn-primary' title='Set Destination Point'><span class='fa fa-map-pin'></span></a></td>" +
                        "</tr>";

            string html = "";
            List<string> lstRows = new List<string>();
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=" + q + "&inputtype=textquery&fields=formatted_address,name,geometry&key=" + Constants.GOOGLE_API_KEY;
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject googleResponse = JObject.Parse(content);
                    JArray googlePlaces = (JArray)googleResponse["candidates"];
                    foreach (var item in googlePlaces)
                    {
                        string name = (string)item["name"];
                        string address = (string)item["formatted_address"];
                        JObject location = (JObject)item["geometry"]["location"];
                        string lat = (string)location["lat"];
                        string lng = (string)location["lng"];
                        lstRows.Add(String.Format(rowFormat, name, address, lat, lng));
                    }
                }
                else
                {
                    lstRows.Add(String.Format(rowFormat, "No Places are found", "Please write name correctly", String.Empty, String.Empty));
                }
            }
            catch (Exception)
            {
                lstRows.Add(String.Format(rowFormat, "No Places are found", "Please write name correctly", String.Empty, String.Empty));
            }
            html = String.Join("\n", lstRows);
            return html;
        }
    }
}