using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
namespace Online_Learning.Controllers
{
    public class AdminHomeController : Controller
    {
        
        OLearningEntities userRepo  = new OLearningEntities();
        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult GetData()
        {
            int admin = userRepo.Users.Where(x => x.UserType == "admin").Count();
            int teacher = userRepo.Users.Where(x => x.UserType == "teacher").Count();
            int student = userRepo.Users.Where(x => x.UserType == "student").Count();
            Ratio obj = new Ratio();
            obj.admin= admin;
            obj.teacher = teacher;
            obj.student = student;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int admin { get; set; }
            public int teacher{ get; set; }
            public int student { get; set; }

        }

        public ActionResult GetStdIns()
        {
            int aiub = userRepo.OnlineStudents.Where(x => x.StdInstitute == "AIUB").Count();
            int du = userRepo.OnlineStudents.Where(x => x.StdInstitute == "DU").Count();
            int ru = userRepo.OnlineStudents.Where(x => x.StdInstitute == "RU").Count();
            int sust = userRepo.OnlineStudents.Where(x => x.StdInstitute == "SUST").Count();
            int rmc = userRepo.OnlineStudents.Where(x => x.StdInstitute == "RMC").Count();
            Rati ob = new Rati();
            ob.AIUB = aiub;
            ob.DU = du;
            ob.RU = ru;
            ob.SUST = sust;
            ob.RMC = rmc;
            

            return Json(ob, JsonRequestBehavior.AllowGet);
        }
        public class Rati
        {
            public int AIUB { get; set; }
            public int DU { get; set; }
            public int SUST { get; set; }

            public int RMC { get; set; }
            public int RU { get; set; }

        }
        [HttpGet]
        public ActionResult ProfileInfo()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User u = userRepo.Users.Where(x => x.UserName == name).FirstOrDefault();
            return View(u);
        }

        [HttpGet]

        public ActionResult Ins()
        {
            return View();
        }

        [HttpGet]

        public ActionResult Financials()
        {
            int tota = 0;
            List<Registration> reg = userRepo.Registrations.ToList();
            foreach (var tk in reg)
            {
                tota += (int)tk.Fee;
            }
            ViewBag.Msg = tota;
            return View(reg);
            
        }

        
        public ActionResult Ok()
        {
            int total = 0;
            List<Registration> reg = userRepo.Registrations.ToList();
            foreach (var tk in reg)
            {
                total += (int)tk.Fee;
            }
            ViewBag.Msg = total;
           
            return View(reg);
        }
        public ActionResult PrintReg()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var report = new ActionAsPdf("Ok");
            return report;
        }

        [HttpGet]

        public ActionResult TFinancials()
        {
            int Stotal = 0;

            List<TeacherFinancial> TF= userRepo.TeacherFinancials.ToList();
            foreach(var t in TF)
            {
                Stotal += (int)t.Salary;
            }
            ViewBag.TMsg = Stotal;
            int Etotal = 0;
            List<Registration> reg = userRepo.Registrations.ToList();
            foreach (var tk in reg)
            {
                Etotal += (int)tk.Fee;
            }
            ViewBag.Msg = Stotal;
            int profit = Etotal - Stotal;
            ViewBag.Profit = profit;
            return View(userRepo.TeacherFinancials.ToList());
        }

        public ActionResult Ko()
        {
            int Stotal = 0;

            List<TeacherFinancial> TF = userRepo.TeacherFinancials.ToList();
            foreach (var t in TF)
            {
                Stotal += (int)t.Salary;
            }
            ViewBag.TMsg = Stotal;
            int Etotal = 0;
            List<Registration> reg = userRepo.Registrations.ToList();
            foreach (var tk in reg)
            {
                Etotal += (int)tk.Fee;
            }
            ViewBag.Msg = Stotal;
            int profit = Etotal - Stotal;
            ViewBag.Profit = profit;
            return View(userRepo.TeacherFinancials.ToList());
        }
        public ActionResult PrintFin()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var report = new ActionAsPdf("Ko");
            return report;
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User t = userRepo.Users.Where(x => x.UserName == name).FirstOrDefault();
            User[] users = userRepo.Users.ToArray();
            ViewData["users"] = users;
            return View(t);
        }
        [HttpPost]
        public ActionResult Edit(User t)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User userToUpdate = userRepo.Users.Where(x => x.UserName == name).FirstOrDefault();
            userToUpdate.UserName = t.UserName;
            userToUpdate.Password = t.Password;
            userToUpdate.UserType = t.UserType;
            userRepo.SaveChanges();
            return RedirectToAction("ProfileInfo");
        }
        [HttpGet]
        public ActionResult Blogs()
        {
            return View(userRepo.MyMaterials.ToList());
        }
        [HttpGet]
        public ActionResult Messages()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllUser()
        {
            List<User> lst=userRepo.Users.Where(p => p.UserType == "Student" || p.UserType == "Teacher").ToList();
            return View(lst);
        }
        [HttpGet]

        public ActionResult Ban(int id)
        {
            User checkban = userRepo.Users.Where(p => p.UserId == id).FirstOrDefault();
            checkban.Status = "Ban";
            userRepo.SaveChanges();
            return RedirectToAction("Banned");
        }
        [HttpGet]

        public ActionResult Banned()
        {
            
            return View(userRepo.Users.Where(p => p.Status == "Ban").ToList());
        }

        [HttpGet]

        public ActionResult UnBan(int id)
        {
            User checkban = userRepo.Users.Where(p => p.UserId == id).FirstOrDefault();
            checkban.Status = " ";
            userRepo.SaveChanges();
            return RedirectToAction("AllUser");
        }

    }
}