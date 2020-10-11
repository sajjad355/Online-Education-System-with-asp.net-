using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Learning.Controllers
{
    public class AdminStudentsController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View(userRepo.OnlineStudents.ToList());
        }
        [HttpPost]
        public ActionResult Index(string searching)
        {
            return View(userRepo.OnlineStudents.Where(x => x.StudentName.Contains(searching) || searching == null).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            OnlineStudent[] students = userRepo.OnlineStudents.ToArray();
            ViewData["students"] = students;
            return View();
        }
        [HttpPost]
        public ActionResult Create(OnlineStudent s)
        {
            userRepo.OnlineStudents.Add(s);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            OnlineStudent s = userRepo.OnlineStudents.Where(x => x.StudentId == id).FirstOrDefault();
            s.StudentId = id;
            OnlineStudent[] students = userRepo.OnlineStudents.ToArray();
            ViewData["students"] = students;
            return View(s);
        }
        [HttpPost]
        public ActionResult Edit(OnlineStudent t, int id)
        {
            OnlineStudent StudentToUpdate = userRepo.OnlineStudents.Where(x => x.StudentId == id).FirstOrDefault();
            StudentToUpdate.StudentId = id;
            StudentToUpdate.StudentName = t.StudentName;
            //StudentToUpdate.Email = t.Email;
            StudentToUpdate.MobileNo = t.MobileNo;
            StudentToUpdate.Address = t.Address;
            StudentToUpdate.StdInstitute = t.StdInstitute;
            //StudentToUpdate.JoinDate = t.JoinDate;
            //StudentToUpdate.EnrolledCourse = t.EnrolledCourse;
           // StudentToUpdate.Status = t.Status;
            //StudentToUpdate.ImageFile = t.ImageFile;
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            OnlineStudent s = userRepo.OnlineStudents.Where(x => x.StudentId == id).FirstOrDefault();
            s.StudentId = id;
            OnlineStudent[] students = userRepo.OnlineStudents.ToArray();
            ViewData["student"] = students;
            return View(s);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            OnlineStudent s = userRepo.OnlineStudents.Where(x => x.StudentId == id).FirstOrDefault();
            return View(s);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            OnlineStudent s = userRepo.OnlineStudents.Where(x => x.StudentId == id).FirstOrDefault();
            userRepo.OnlineStudents.Remove(s);
            userRepo.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}