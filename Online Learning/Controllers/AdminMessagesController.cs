using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Learning.Controllers
{
    public class AdminMessagesController : Controller
    {
        OLearningEntities userRepo = new OLearningEntities();
        [HttpGet]
        public ActionResult Index()
        {/*
            return View(userRepo.Messages.ToList());*/
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<Message> msg = userRepo.Messages.Where(b => b.ReceiverName == name || b.SenderName == name).ToList();
            return View(msg);
        }
        [HttpPost]
        public ActionResult Index(string searching)
        {
            return View(userRepo.Messages.Where(x => x.SenderName.Contains(searching) || searching == null).ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            Message[] messages = userRepo.Messages.ToArray();
            ViewData["messages"] = messages;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Message m)
        {
            Message mg = new Message();
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User getTy = userRepo.Users.Where(l => l.UserName == name).FirstOrDefault();
            mg.SenderName = Session["Username"].ToString();
            mg.SenderType = getTy.UserType;
            mg.ReceiverName = m.ReceiverName;
            mg.Text = m.Text;
            userRepo.Messages.Add(mg);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Message c = userRepo.Messages.Where(x => x.MessageId == id).FirstOrDefault();
            c.MessageId = id;
            Message[] messages = userRepo.Messages.ToArray();
            ViewData["messages"] = messages;
            return View(c);
        }
        [HttpPost]
        public ActionResult Edit(Message c, int id)
        {
            userRepo.Messages.Add(c);
            userRepo.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Message m = userRepo.Messages.Where(x => x.MessageId == id).FirstOrDefault();
            return View(m);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Message m = userRepo.Messages.Where(x => x.MessageId == id).FirstOrDefault();
            userRepo.Messages.Remove(m);
            userRepo.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}