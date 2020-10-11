using Online_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Web.WebPages;

namespace Online_Learning.Controllers
{
    public class StudentController : Controller
    {
        OLearningEntities db = new OLearningEntities();

        [HttpGet]
        public ActionResult AllCourses()
        {
            List<Subject> s = db.Subjects.ToList();

            return View(s);
        }

        [HttpPost]
        public ActionResult AllCourses(string searching)
        {
            return View(db.Subjects.Where(x => x.SubjectName.Contains(searching) || searching == null).ToList());
        }

        [HttpGet]

        public ActionResult MyCourses()
        {
            if (Session["Username"]==null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<CoursesOfStudent> co = db.CoursesOfStudents.Where(p => p.StudentName == name).ToList();

            return View(co);
        }
        

        [HttpGet]

        public ActionResult Enroll(int id)
        {
            CoursesOfStudent cos = new CoursesOfStudent();
            Subject ss = db.Subjects.Where(p => p.SubjectId == id).FirstOrDefault();
           
                cos.CourseName = ss.SubjectName;
                if (Session["Username"] == null)
                {
                    return RedirectToAction("Login", "Login");
                }
                cos.StudentName = Session["Username"].ToString();

                return View(cos);
        
        }

        [HttpPost, ActionName("Enroll")]

        public ActionResult ConfirmEnroll(int id)
        {
            CoursesOfStudent cos = new CoursesOfStudent();
            Subject ss = db.Subjects.Where(p => p.SubjectId == id).FirstOrDefault();
            cos.CourseName = ss.SubjectName;
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            cos.StudentName = Session["Username"].ToString();
            cos.CourseId = id;
            cos.TeacherId = ss.TeacherId;
           
            db.CoursesOfStudents.Add(cos);
            ss.StudentCount += 1;
            db.SaveChanges();

            Registration re = new Registration();

            re.SubjectId = ss.SubjectId;
            re.StudentName = Session["Username"].ToString();
            re.Fee = ss.Price;
            db.Registrations.Add(re);
            db.SaveChanges();
            return RedirectToAction("AllCourses");

        }

        [HttpGet]
        public ActionResult Registration()
        {
            int total=0;
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<Registration> rl = db.Registrations.Where(x => x.StudentName == name).ToList();
            foreach(var tk in rl)
            {
                total += (int)tk.Fee;
            }
            ViewBag.Msg = total;
            ViewBag.APaid = 0;
            ViewBag.Due = total;
            int APaid = 0;
            List<Payment>  py = db.Payments.Where(v => v.StudentName == name).ToList();
            if(py!=null)
            {
                ViewBag.Msg = total;
                foreach(var am in py)
                {
                    APaid += (int) am.Amount;
                }
                ViewBag.APaid = APaid;
                ViewBag.Due = total-APaid;
            }
            
            return View(rl);
        }

        [HttpGet]
        public ActionResult PayOnline()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PayOnline(int? id=null)
        {
            int total = 0;
            Payment pm = new Payment();
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            string name = Session["Username"].ToString();
            List<Registration> rl = db.Registrations.Where(x => x.StudentName == name).ToList();
            foreach (var tk in rl)
            {
                total += (int)tk.Fee;
            }

            pm.StudentName = name;
            pm.Amount = total;

            
            Payment pCheck= db.Payments.Where(c => c.StudentName == name).FirstOrDefault();
            
            if(pCheck!=null)
            {
               Registration rCheck = db.Registrations.Where(x => x.StudentName == name).OrderByDescending(x => x.StudentName == name).FirstOrDefault();

                pm.Amount = rCheck.Fee;
            }

            db.Payments.Add(pm);

            db.SaveChanges();
            return RedirectToAction("Registration");
        }

        [HttpGet]
        public ActionResult MyProfile()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User u = db.Users.Where(x => x.UserName == name).FirstOrDefault();
            return View(u);
          

        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User t = db.Users.Where(x => x.UserName == name).FirstOrDefault();
            User[] users = db.Users.ToArray();
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
            User userToUpdate = db.Users.Where(x => x.UserName == name).FirstOrDefault();
            userToUpdate.UserName = t.UserName;
            userToUpdate.Password = t.Password;
            userToUpdate.UserType = t.UserType;
            db.SaveChanges();
            return RedirectToAction("ProfileInfo");
        }

        [HttpGet]

        public ActionResult StudentEdit()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string Uid = Session["UserID"].ToString();
            int uuid = Convert.ToInt32(Uid) ;
            List<OnlineStudent> update = db.OnlineStudents.Where(x => x.UserId == uuid).ToList();

            return View(update);
        }

        [HttpGet]

        public ActionResult EditMore()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string Uid = Session["UserID"].ToString();
            int uuid = Convert.ToInt32(Uid);
            OnlineStudent update = db.OnlineStudents.Where(x => x.UserId == uuid).FirstOrDefault();

            return View(update);
        }


        [HttpPost]

        public ActionResult EditMore(OnlineStudent os)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string Uid = Session["UserID"].ToString();
            int uuid = Convert.ToInt32(Uid);
            OnlineStudent update = db.OnlineStudents.Where(x => x.UserId == uuid).FirstOrDefault();
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User pp = db.Users.Where(p => p.UserName == name).FirstOrDefault();
            update.UserId = pp.UserId;
            update.StudentName = pp.UserName;
            update.MobileNo = os.MobileNo;
            update.StdInstitute = os.StdInstitute;
            update.Address = os.Address;
            db.SaveChanges();
            return RedirectToAction("StudentEdit");
        }

        [HttpGet]
        public ActionResult CreateMore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMore(OnlineStudent oo)
        {
            OnlineStudent update = new OnlineStudent();
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            User pp = db.Users.Where(p => p.UserName == name).FirstOrDefault();
            update.UserId = pp.UserId;
            update.StudentName = pp.UserName;
            update.MobileNo = oo.MobileNo;
            update.StdInstitute = oo.StdInstitute;
            update.Address = oo.Address;
            db.OnlineStudents.Add(update);

            db.SaveChanges();
            return RedirectToAction("StudentEdit");
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

            OnlineStudent profileToUpdate = db.OnlineStudents.Where(x => x.UserId == id).FirstOrDefault();

            profileToUpdate.ImageName = FileName;
            profileToUpdate.ImagePath = FilePath;
            file.SaveAs(FilePath);
            db.SaveChanges();

            return RedirectToAction("StudentEdit");


        }

        [HttpGet]

        public ActionResult ShowMessage()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            string name = Session["Username"].ToString();
            List<Message> msg = db.Messages.Where(b => b.ReceiverName == name || b.SenderName==name).ToList();
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
            User getTy=db.Users.Where(l => l.UserName == name).FirstOrDefault();
            mg.SenderName = Session["Username"].ToString();
            mg.SenderType = getTy.UserType;
            mg.ReceiverName = m.ReceiverName;
            mg.Text = m.Text;
            db.Messages.Add(mg);
            db.SaveChanges();
            return RedirectToAction("ShowMessage");
        }
        [HttpGet]
        public ActionResult Notes(int id)
        {
            List<MyMaterial> mm = db.MyMaterials.Where(x => x.SubjectId == id).ToList();
            if(mm!=null)
            {
                return View(mm);
            }
            else { return RedirectToAction("MyCourses"); }
        }
        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~/Uploaded/" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        public ActionResult ShowVideo(int id)
        {
            List<Video> n = db.Videos.Where(x => x.SubjectId == id).ToList();

            return View(n);
        }
     

    }
}