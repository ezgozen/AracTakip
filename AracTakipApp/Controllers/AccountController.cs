using AracTakipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracTakipApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (AracTakipDBOEntities db = new AracTakipDBOEntities())
            {
                return View(db.UserAccount.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string firstName, string lastName, string password, string confirmpassword)
        {
            UserAccount user = new UserAccount();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Password = password;
            user.ConfirmPassword = confirmpassword;
            user.UserName = user.FirstName.ToLower().ToString() + '.' + user.LastName.ToLower().ToString();
            if (ModelState.IsValid)
            {
                using (AracTakipDBOEntities db = new AracTakipDBOEntities())
                {
                    if (db.UserAccount.Any(x => x.UserName == user.UserName))
                    {
                        ViewBag.DublicateMessage = "User Name already exists.";
                    }
                    else
                    {
                        db.UserAccount.Add(user);
                        db.SaveChanges();
                    }
                }
                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " succesfully created.";
            }
            return View();
        }

        //Login
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("Login/{user?}")]
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (AracTakipDBOEntities db = new AracTakipDBOEntities())
            {
                var usr = db.UserAccount.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    return RedirectToAction("Create", "Devices");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is wrong.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            if (Session["UserID"] != null)
            {
                Session["UserID"] = null;
                Session["UserName"] = null;
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}
