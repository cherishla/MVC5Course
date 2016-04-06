using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HandleError(ExceptionType =typeof(InvalidOperationException), View ="Error2")]
        public ActionResult About(string name)
        {
            ViewBag.Message = "Your application description page.";
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Argument is Error");
            }
            throw new InvalidOperationException("Just error!");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}