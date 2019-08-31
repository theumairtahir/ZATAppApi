using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZATAppApi.ViewModels;
using ZATAppApi.Models;
using ZATAppApi.Common;

namespace ZATAppApi.Controllers
{
    public class RegisterationController : Controller
    {
        /// <summary>
        /// Action to return the view for create a new sub-admin
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterSubAdmin()
        {
            return View();
        }
        /// <summary>
        /// POST method to be called on form submission of register sub-admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterSubAdmin(RegisterSubAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            try
            {
                //personal details
                SubAdmin subAdmin = new SubAdmin(new User.NameFormat
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                }, new User.ContactNumberFormat(model.CountryCode, model.CompanyCode, model.Number));
                //areas
                List<string> lstAreas = new List<string>();
                string temp = "";
                foreach (var item in model.Areas.ToCharArray())
                {
                    if (item == ',')
                    {
                        lstAreas.Add(temp);
                        temp = "";
                    }
                    else
                    {
                        temp += item;
                    }
                }
                lstAreas.Add(temp);
                foreach (var item in lstAreas)
                {
                    subAdmin.AddArea(item);
                }
                //register identity
                subAdmin.RegisterIdentityUser(model.Username, Constants.DEFAULT_PASSWORD);
                return View("RegisterationConfirmation", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}