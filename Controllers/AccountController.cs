using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeCare.Models;
using EmployeeCare.ViewModel;
using EmployeeCare.Enum;

namespace EmployeeCare.Controllers
{
    public class AccountController : Controller
    {
        EmployeeCareDbContext db = new EmployeeCareDbContext();
        // GET: Account
        public ActionResult Login()
        {
            ViewBag.errorMsg = TempData["errorMsg"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            User currentUser = db.Users.Where(s => s.user_name.ToLower() == user.user_name.ToLower() && s.password == user.password).FirstOrDefault();
            if(currentUser != null)
            {
                if (currentUser.active == (int)UserActiveStatus.Active)
                {
                    Session["user"] = currentUser;
                    Session["type"] = currentUser.type;
                    Session["id"] = currentUser.id;
                    Session["user_name"] = currentUser.user_name;
                    Session["image"] = !String.IsNullOrEmpty(currentUser.image)?currentUser.image:null ;
                }
                else
                {
                    TempData["errorMsg"] = EmployeeCare.Resources.ar.user_not_actived;
                    return RedirectToAction("Login");
                }

            }
            else
            {
                TempData["errorMsg"] = EmployeeCare.Resources.ar.invalid_username_password;
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}