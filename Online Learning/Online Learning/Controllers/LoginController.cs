using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Online_Learning.Controllers
{
    public class LoginController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();

        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {

            using (OLearningEntities data = new OLearningEntities())
            {
                var obj = data.Users.Where(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    if(obj.Status=="Ban")
                    {
                        ViewBag.Ban = "You are banned contact admin@edusys.edu";
                        return View();
                    }
                    Session["UserID"] = obj.UserId.ToString();
                    Session["Username"] = obj.UserName.ToString();
                    //Session["UserType"] = obj.UserType.ToString();
                    if (obj.UserType.ToString() == "Admin")

                        return RedirectToAction("AdminDashboard");

                    else if (obj.UserType.ToString() == "Teacher")

                        return RedirectToAction("Index", "TeacherHome");

                    else if (obj.UserType.ToString() == "Student")

                        return RedirectToAction("StudentDashboard");

                }
                else 
                {
                    ViewBag.Error = "Invalid User Name or Password";

                   // return View();
                }

                return View();
            }
             
            //return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TeacherDashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StudentDashboard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            User[] users = userRepo.Users.ToArray();
            ViewData["users"] = users;
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User u)
        {
            userRepo.Users.Add(u);
            userRepo.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}