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

        [Authorize]
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                // Admin admin = HttpContext.Session.Get<Admin>("admin");
                return View();
            }
            else
            {
                var val = TempData.Get<Admin>("admin4ik");
                if (val == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                HttpContext.Session.Set("admin", val);
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
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
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddSubject()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddSubject(Subject subject)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
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
