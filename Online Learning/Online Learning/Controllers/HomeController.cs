using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Learning.Controllers
{
    public class HomeController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            Message[] messages = userRepo.Messages.ToArray();
            ViewData["messages"] = messages;
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Message m)
        {
            userRepo.Messages.Add(m);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}