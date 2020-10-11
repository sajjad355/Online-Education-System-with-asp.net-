using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Learning.Controllers
{
    public class AdminCourseController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View(userRepo.Subjects.ToList());
        }
        [HttpPost]
        public ActionResult Index(string searching)
        {
            return View(userRepo.Subjects.Where(x => x.SubjectName.Contains(searching) || searching == null).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            Subject[] subjects = userRepo.Subjects.ToArray();
            ViewData["courses"] = subjects;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Subject c)
        {
            userRepo.Subjects.Add(c);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Subject c = userRepo.Subjects.Where(x => x.SubjectId == id).FirstOrDefault();
            c.SubjectId = id;
            Subject[] courses = userRepo.Subjects.ToArray();
            ViewData["courses"] = courses;
            return View(c);
        }
        [HttpPost]
        public ActionResult Edit(Subject c, int id)
        {
            Subject courseToUpdate = userRepo.Subjects.Where(x => x.SubjectId == id).FirstOrDefault();
            courseToUpdate.SubjectId = id;
            courseToUpdate.SubjectName = c.SubjectName;
            courseToUpdate.Description = c.Description;
            courseToUpdate.TeacherId = c.TeacherId;
            courseToUpdate.SubjectType = c.SubjectType;
            courseToUpdate.Price = c.Price;
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            Subject c = userRepo.Subjects.Where(x => x.SubjectId == id).FirstOrDefault();
            c.SubjectId = id;
            Subject[] courses = userRepo.Subjects.ToArray();
            ViewData["courses"] = courses;
            return View(c);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Subject p = userRepo.Subjects.Where(x => x.SubjectId == id).FirstOrDefault();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Subject p = userRepo.Subjects.Where(x => x.SubjectId == id).FirstOrDefault();
            userRepo.Subjects.Remove(p);
            userRepo.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}