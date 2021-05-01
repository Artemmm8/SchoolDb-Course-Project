using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDbProject.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Xml;
using System.Threading.Tasks;
using SchoolDbProject.LoginAndRegistraionModels;

namespace SchoolDbProject.Controllers
{
    public class AdminController : Controller
    {
        private SchoolDbContext db;
        private IWebHostEnvironment _appEnvironment;

        public AdminController(SchoolDbContext db, IWebHostEnvironment appEnvironment)
        {
            this.db = db;
            _appEnvironment = appEnvironment;
        }

        // [Authorize]
        public IActionResult Index()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                // Admin admin = HttpContext.Session.Get<Admin>("admin");
                return View();
            //}
            //else
            //{
            //    var val = TempData.Get<Admin>("admin4ik");
            //    if (val == null)
            //    {
            //        return RedirectToAction("Logout", "Account");
            //    }

            //    HttpContext.Session.Set("admin", val);
            //    return View();
            //}
        }

        // [Authorize]
        public IActionResult AddSubjectByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddSubjectByForm(Subject subject)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                int id = db.Subjects.Max(s => s.SubjectId) + 1;
                subject.SubjectId = id;
                db.Subjects.Add(subject);
                db.SaveChanges();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddSubjectByXml()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            

            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddSubjectByXml(IFormFile file)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                var path = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList dataNodes = doc.SelectNodes("/subjects/subject");
                    int count = 0;
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        Subject subject = db.Subjects.FirstOrDefault(s => s.SubjectId == id);
                        if (subject == null)
                        {
                            count++;
                        }
                    }

                    if (count == dataNodes.Count)
                    {
                        foreach (XmlNode item in dataNodes)
                        {
                            int id = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                            string name = item.SelectSingleNode("SubjectName").InnerText;
                            db.Subjects.Add(new Subject { SubjectId = id, SubjectName = name });
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "xml doc has incorrect data.";
                    }

                    System.IO.File.Delete(path);
                }

            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddClassByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddClassByForm(Class class1)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                int id = db.Classes.Max(c => c.ClassId) + 1;
                class1.ClassId = id;
                db.Classes.Add(class1);
                db.SaveChanges();
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddClassByXml()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddClassByXml(IFormFile file)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                var path = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList dataNodes = doc.SelectNodes("/classes/class");
                    int count = 0;
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        Class class1 = db.Classes.FirstOrDefault(c => c.ClassId == id);
                        if (class1 == null)
                        {
                            count++;
                        }
                    }

                    if (count == dataNodes.Count)
                    {
                        foreach (XmlNode item in dataNodes)
                        {
                            int id = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                            string name = item.SelectSingleNode("ClassName").InnerText;
                            byte numberOfStudents = Convert.ToByte(item.SelectSingleNode("NumberOfStudents").InnerText);
                            db.Classes.Add(new Class { ClassId = id, ClassName = name, NumberOfStudents = numberOfStudents });
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "xml doc has incorrect data.";
                    }

                    System.IO.File.Delete(path);
                }

                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddCabinetByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddCabinetByForm(Cabinet cabinet)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                int id = db.Cabinets.Max(s => s.CabinetId) + 1;
                cabinet.CabinetId = id;
                db.Cabinets.Add(cabinet);
                db.SaveChanges();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddCabinetByXml()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddCabinetByXml(IFormFile file)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                var path = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList dataNodes = doc.SelectNodes("/cabinets/cabinet");
                    int count = 0;
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        Cabinet cabinet = db.Cabinets.FirstOrDefault(c => c.CabinetId == id);
                        if (cabinet == null)
                        {
                            count++;
                        }
                    }

                    if (count == dataNodes.Count)
                    {
                        foreach (XmlNode item in dataNodes)
                        {
                            int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                            byte numberOfSeats = Convert.ToByte(item.SelectSingleNode("NumberOfSeats").InnerText);
                            db.Cabinets.Add(new Cabinet { CabinetId = id, NumberOfSeats = numberOfSeats });
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "xml doc has incorrect data.";
                    }

                    System.IO.File.Delete(path);
                }

            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddTeacherByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddTeacherByForm(Teacher teacher)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                teacher.Password = AccountController.HashPassword(teacher.Password);
                db.Teachers.Add(teacher);
                db.SaveChanges();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddTeacherByXml(IFormFile file)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                var path = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList dataNodes = doc.SelectNodes("/cabinets/cabinet");
                    int count = 0;
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        Cabinet cabinet = db.Cabinets.FirstOrDefault(c => c.CabinetId == id);
                        if (cabinet == null)
                        {
                            count++;
                        }
                    }

                    if (count == dataNodes.Count)
                    {
                        foreach (XmlNode item in dataNodes)
                        {
                            int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                            byte numberOfSeats = Convert.ToByte(item.SelectSingleNode("NumberOfSeats").InnerText);
                            db.Cabinets.Add(new Cabinet { CabinetId = id, NumberOfSeats = numberOfSeats });
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "xml doc has incorrect data.";
                    }

                    System.IO.File.Delete(path);
                }

                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        public IActionResult AddStudentByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            HashSet<string> classes = new HashSet<string>();
            foreach (var c in db.Classes)
            {
                classes.Add(c.ClassName);
            }

            return View(new CustomStudent { Classes = new List<string>(classes) });
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddStudentByForm(CustomStudent customStudent)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                customStudent.Password = AccountController.HashPassword(customStudent.Password);
                Student student = new Student { StudentId = Convert.ToInt32(customStudent.StudentId), Email = customStudent.Email, 
                    Password = customStudent.Password };
                int? classId = db.Classes.FirstOrDefault(c => c.ClassName == customStudent.ClassName).ClassId;
                student.ClassId = classId;
                db.Students.Add(student);
                db.SaveChanges();
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        public IActionResult AddStudentByXml()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{


                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        // [Authorize]
        [HttpPost]
        public IActionResult AddStudentByXml(IFormFile file)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                var path = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList dataNodes = doc.SelectNodes("/cabinets/cabinet");
                    int count = 0;
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        Cabinet cabinet = db.Cabinets.FirstOrDefault(c => c.CabinetId == id);
                        if (cabinet == null)
                        {
                            count++;
                        }
                    }

                    if (count == dataNodes.Count)
                    {
                        foreach (XmlNode item in dataNodes)
                        {
                            int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                            byte numberOfSeats = Convert.ToByte(item.SelectSingleNode("NumberOfSeats").InnerText);
                            db.Cabinets.Add(new Cabinet { CabinetId = id, NumberOfSeats = numberOfSeats });
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "xml doc has incorrect data.";
                    }

                    System.IO.File.Delete(path);
                }

                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        public IActionResult AddMarkByForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMarkByForm(CustomMarks customMarks)
        {
            return View();
        }

        public IActionResult AddMarkByXml()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMarkByXml(IFormFile formFile)
        {
            return View();
        }

        // [Authorize]
        public IActionResult Export()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);
                XmlNode classesNode = doc.CreateElement("classes");
                doc.AppendChild(classesNode);
                foreach (var item in db.Classes)
                {
                    XmlNode classNode = doc.CreateElement("class");
                    classesNode.AppendChild(classNode);
                    XmlNode idNode = doc.CreateElement("ClassId");
                    idNode.AppendChild(doc.CreateTextNode(item.ClassId.ToString()));
                    classNode.AppendChild(idNode);
                    XmlNode nameNode = doc.CreateElement("ClassName");
                    nameNode.AppendChild(doc.CreateTextNode(item.ClassName));
                    classNode.AppendChild(nameNode);
                    XmlNode numNode = doc.CreateElement("ClassId");
                    numNode.AppendChild(doc.CreateTextNode(item.NumberOfStudents.ToString()));
                    classNode.AppendChild(numNode);
                }

                doc.Save("classes.xml");
                return RedirectToAction("Index" , "Admin");
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }
    }
}