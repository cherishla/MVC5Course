using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Course.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            if(CheckLogin(data))
            {
                //一般都用session ，所以cookie設定為false
                FormsAuthentication.RedirectFromLoginPage(data.Email, false);
            }
            return View();
        }

        private bool CheckLogin(LoginViewModel data)
        {
            return(data.Email =="lala@edetw.com" && data.Password=="123");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel data)
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditProfile()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel data)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}