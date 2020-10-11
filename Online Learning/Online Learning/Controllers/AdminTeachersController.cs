using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Learning.Controllers
{
    public class AdminTeachersController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View(userRepo.Teachers.ToList());
        }
        [HttpPost]
        public ActionResult Index(string searching)
        {
            return View(userRepo.Teachers.Where(x => x.TeacherName.Contains(searching) || searching == null).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            Teacher[] teachers = userRepo.Teachers.ToArray();
            ViewData["teachers"] = teachers;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Teacher t)
        {
            userRepo.Teachers.Add(t);
            userRepo.SaveChanges();

            TeacherFinancial financial = new TeacherFinancial();
            financial.TeacherId = t.TeacherId;
            financial.Salary = (double)t.Salary;
            userRepo.TeacherFinancials.Add(financial);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Teacher t = userRepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            t.TeacherId = id;
            Teacher[] teachers = userRepo.Teachers.ToArray();
            ViewData["teachers"] = teachers;
            return View(t);
        }
        [HttpPost]
        public ActionResult Edit(Teacher t, int id)
        {
            Teacher teacherToUpdate = userRepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            teacherToUpdate.TeacherId = id;
            teacherToUpdate.TeacherName = t.TeacherName;
            //teacherToUpdate. = t.Email;
            teacherToUpdate.MobileNo = t.MobileNo;
            teacherToUpdate.Address = t.Address;
            teacherToUpdate.Institute = t.Institute;
            //teacherToUpdate.JoiningDate = t.JoiningDate;
            teacherToUpdate.Salary = t.Salary;
            //teacherToUpdate.Status = t.Status;
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            Teacher t = userRepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            t.TeacherId = id;
            Teacher[] teachers = userRepo.Teachers.ToArray();
            ViewData["teachers"] = teachers;
            return View(t);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Teacher t = userRepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            return View(t);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Teacher t = userRepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            userRepo.Teachers.Remove(t);
            userRepo.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}