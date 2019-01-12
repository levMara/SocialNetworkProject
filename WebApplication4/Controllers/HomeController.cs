using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Controllers
{
    public class HomeController : IdentityControllerBase
    {
        public ActionResult Index()
        {
            RefreshToken();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            RefreshToken();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            RefreshToken();
            return View();
        }
    }
}