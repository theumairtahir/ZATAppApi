using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ZATAppApi.ViewModels;
using ZATAppApi.Models;
using ZATAppApi.Common;

namespace ZATAppApi.Controllers
{
    [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Returns a list of users to reset passwords
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPasswords()
        {
            try
            {
                List<ResetPasswordsViewModel> model = new List<ResetPasswordsViewModel>();
                foreach (var item in Models.User.GetAllUsers())
                {
                    if (item.Role == Models.User.ApplicationRoles.Driver || item.Role == Models.User.ApplicationRoles.SubAdmin)
                    {
                        model.Add(new ResetPasswordsViewModel
                        {
                            Contact = item.ContactNumber.LocalFormatedPhoneNumber,
                            Id = item.UserId,
                            Name = item.FullName.FirstName + " " + item.FullName.LastName,
                            IsBlocked = item.IsBlocked
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
        /// Action to reset password for a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(long id)
        {
            try
            {
                User user = new User(id);
                user.ResetPassword(Constants.DEFAULT_PASSWORD);
                ResetPasswordConfirmationViewModel model = new ResetPasswordConfirmationViewModel
                {
                    Name = user.FullName.FirstName + " " + user.FullName.LastName,
                    Password = Constants.DEFAULT_PASSWORD,
                    Username = user.GetApplicationUser().UserName
                };

                return View("ResetPasswordConfirmation", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to Block a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Block(long id)
        {
            try
            {
                User user = new User(id)
                {
                    IsBlocked = true
                };
                return RedirectToAction("ResetPasswords");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
        /// <summary>
        /// Action to Unblock a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Unblock(long id)
        {
            try
            {
                User user = new User(id)
                {
                    IsBlocked = false
                };
                return RedirectToAction("ResetPasswords");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error", ex);
            }
        }
    }
}