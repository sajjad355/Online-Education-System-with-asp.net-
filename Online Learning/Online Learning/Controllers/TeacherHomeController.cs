using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Online_Learning.Controllers
{
    public class TeacherHomeController : Controller
    {

        OLearningEntities erepo = new OLearningEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        //Show All course
        public ActionResult ShowAllSubject()
        {
            return View(erepo.Subjects.ToList());
        }

        //[HttpGet]

        // Teacher Profile View
        [HttpGet]
        public ActionResult MyProfile()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();

            List<Teacher> teacherToView = erepo.Teachers.Where(p => p.UserName == name).ToList();


            return View(teacherToView);
        }

        //Teacher Profile Edit
        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();

            Teacher profileToUpdate = erepo.Teachers.Where(p => p.UserName == name).FirstOrDefault();

            return View(profileToUpdate);
        }


        [HttpPost]
        public ActionResult EditProfile(Teacher t, int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();

            Teacher profileToUpdate = erepo.Teachers.Where(p => p.UserName == name).FirstOrDefault();

            
            //profileToUpdate.TeacherId = id;
            profileToUpdate.TeacherName = t.TeacherName;
            profileToUpdate.MobileNo = t.MobileNo;
            profileToUpdate.Institute = t.Institute;
            profileToUpdate.Address = t.Address;
            //profileToUpdate.ImageFile = t.ImageFile;


            erepo.SaveChanges();


            return RedirectToAction("MyProfile");
        }

        [HttpGet]

        public ActionResult UploadPro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPro(HttpPostedFileBase file, int id)
        {
            string FileName = Path.GetFileName(file.FileName);
            string FilePath = Path.Combine(Server.MapPath("~/Uploaded/ProImage/"), FileName);

            Teacher profileToUpdate = erepo.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();

            profileToUpdate.ImageName = FileName;
            profileToUpdate.ImagePath = FilePath;
            file.SaveAs(FilePath);
            erepo.SaveChanges();

            return RedirectToAction("MyProfile");


        }

        //Information of all Students 
        [HttpGet]
        public ActionResult AllStudents()
        {
            return View(erepo.OnlineStudents.ToList());
        }

        

        [HttpGet]
        public ActionResult CreateCourse(int id)
        {

            return View();
        }


        [HttpPost]
        public ActionResult CreateCourse(Subject s, int id)
        {
            Subject ss = erepo.Subjects.Where(p => p.SubjectId == id).FirstOrDefault();

            s.TeacherId = ss.TeacherId;

            erepo.Subjects.Add(s);
            erepo.SaveChanges();

            return View("Index");
        }

        [HttpGet]
        //Show All course
        public ActionResult ShowMySubject()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<Teacher> teacherToView = erepo.Teachers.Where(p => p.UserName == name).ToList();
            int tId = 0;

            foreach (var value in teacherToView)
            {
                tId = value.TeacherId;

            }
            List<Subject> subject = erepo.Subjects.Where(p => p.TeacherId == tId).ToList();


            return View(subject);
        }



        [HttpGet]
        public ActionResult AddNotes(int id)
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult AddNotes(HttpPostedFileBase file, int id)
        {
            var course = erepo.Subjects.Where(z => z.SubjectId == id).FirstOrDefault();

            string FileName = Path.GetFileName(file.FileName);
            string FilePath = Path.Combine(Server.MapPath("~/Uploaded"), FileName);

            MyMaterial content = new MyMaterial();
            content.SubjectId = course.SubjectId;
            content.MaterialName = FileName;
            content.MaterialLink = FilePath;
            file.SaveAs(FilePath);

            //Content con = new Content();
            //con.Course_Id = content.Course_Id;
            //con.Instructor_Id = content.Instructor_Id;

            erepo.MyMaterials.Add(content);
            erepo.SaveChanges();
            return RedirectToAction("AddNotes");
        }
        [HttpGet]
        public ActionResult AddVideos(int id)
        {
        
            return View();
        }

        [HttpPost]
        
        public ActionResult AddVideos(HttpPostedFileBase file, int id, Video v)
        {
            var course = erepo.Subjects.Where(z => z.SubjectId == id).FirstOrDefault();

            if(file!=null)
            {
                
                string FileName = Path.GetFileName(file.FileName);
             
                    string FilePath = Path.Combine(Server.MapPath("~/Uploaded/Videos/"), FileName);

                    Video content = new Video();

                    content.SubjectId = course.SubjectId;
                    content.VideoName = FileName;
                    content.VideoPath = FilePath;
                    content.VideoDescription = v.VideoDescription;
                    file.SaveAs(FilePath);

                    erepo.Videos.Add(content);
                    erepo.SaveChanges();
                    ViewData["display"] = "Uploaded!";
              
          
            }
            return RedirectToAction("AddVideos");
        }

        [HttpGet]

        public ActionResult ShowMessage()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<Message> msg = erepo.Messages.Where(b => b.ReceiverName == name || b.SenderName == name).ToList();
            return View(msg);
        }

        [HttpGet]

        public ActionResult Create()
        {
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
            User getTy = erepo.Users.Where(l => l.UserName == name).FirstOrDefault();
            mg.SenderName = Session["Username"].ToString();
            mg.SenderType = getTy.UserType;
            mg.ReceiverName = m.ReceiverName;
            mg.Text = m.Text;
            erepo.Messages.Add(mg);
            erepo.SaveChanges();
            return RedirectToAction("ShowMessage");
        }

    }
}