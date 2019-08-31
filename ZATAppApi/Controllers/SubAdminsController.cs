using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATApp.Models;
using ZATApp.ViewModels;

namespace ZATAppApi.Controllers
{
    public class SubAdminsController : Controller
    {
        // GET: SubAdmins
        public ActionResult Index()
        {
            List<ViewSubAdminViewModel> model = new List<ViewSubAdminViewModel>();
            foreach (var item in SubAdmin.GetAllSubAdmins())
            {
                model.Add(new ViewSubAdminViewModel(item.GetAllAreas())
                {
                    Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                    Name = item.FullName.FirstName + " " + item.FullName.LastName,
                    Id = item.UserId
                });
            }
            return View(model);
        }
        public ActionResult AddArea(long id)
        {
            try
            {
                AddAreaViewModel model = new AddAreaViewModel
                {
                    SubAdminId = id
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
        public ActionResult AddArea(AddAreaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                SubAdmin subAdmin = new SubAdmin(model.SubAdminId);
                subAdmin.AddArea(model.AreaName);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        public ActionResult RemoveArea(long id)
        {
            try
            {
                SubAdmin subAdmin = new SubAdmin(id);
                List<AreaViewModel> model = new List<AreaViewModel>();
                foreach (var item in subAdmin.GetAllAreas())
                {
                    model.Add(new AreaViewModel
                    {
                        Id = item.AreaId,
                        Name = item.AreaName,
                        SubAdminId = subAdmin.UserId
                    });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        public ActionResult RemoveArea2(long uId, int aId)
        {

            try
            {
                SubAdmin subAdmin = new SubAdmin(uId);
                subAdmin.RemoveArea(new SubAdmin.Area
                {
                    AreaId = aId
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}