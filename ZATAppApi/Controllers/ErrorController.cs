using System;
using System.Web.Mvc;

namespace ZATAppApi.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult ErrorPage(Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}