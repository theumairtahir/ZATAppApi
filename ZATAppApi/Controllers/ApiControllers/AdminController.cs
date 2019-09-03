using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using ZATAppApi.ASPNetIdentity;

namespace ZATAppApi.Controllers.ApiControllers
{
    /// <summary>
    /// Controls the tasks in admin identity
    /// </summary>
    public class AdminController : ApiController
    {
        /// <summary>
        /// Creates a new identity for admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("api/Admin/Create")]
        public IHttpActionResult Create()
        {
            try
            {
                string password = "admin123";
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var user = new ApplicationUser() { UserName = "admin", Email = "test@app.com", PhoneNumber = Common.Constants.ADMIN_PHONE_NUMBER };
                ApplicationUserManager manager = new ApplicationUserManager(userStore);
                var result = manager.Create(user, password);
                if (result.Succeeded)
                {
                    user = manager.FindByName(user.UserName);
                    manager.AddToRole(user.Id, "Admin" + "");

                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Resets the password for the admin user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("api/Admin/ResetPassword")]
        public IHttpActionResult ResetPassword()
        {
            try
            {
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new ApplicationUserManager(store);
                var user = manager.FindByName("admin");
                var resetToken = manager.GeneratePasswordResetToken(user.Id);
                var result = manager.ResetPassword(user.Id, resetToken, "admin123");
                if (!result.Succeeded)
                {
                    Ok(false);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
