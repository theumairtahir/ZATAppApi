using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using ZATApp.ViewModels;
using PagedList;
using ZATApp.Common;

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
        public ActionResult ViewDriverDetails(long id)
        {
            try
            {
                return Content(new Driver(id).CNIC_Number);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}