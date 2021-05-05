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
            HashSet<string> classes = new HashSet<string>();
            foreach (var c in db.Classes)
            {
                classes.Add(c.ClassName);
            }
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            customStudent.Password = AccountController.HashPassword(customStudent.Password);
                Student student = new Student { StudentId = Convert.ToInt32(customStudent.StudentId), Email = customStudent.Email, 
                    Password = customStudent.Password };
                int? classId = db.Classes.FirstOrDefault(c => c.ClassName == customStudent.ClassName).ClassId;
                student.ClassId = classId;
                db.Students.Add(student);
                db.SaveChanges();
            return View(new CustomStudent { Classes = new List<string>(classes) });
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
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
                // Admin admin = HttpContext.Session.Get<Admin>("admin");
                var marksAnonA = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassId, c.ClassName, Name = s.FirstName + " " + s.LastName })
                    .Join(db.StudentSchedules, sc => sc.ClassId, ss => ss.ClassId, (sc, ss) => new { sc.Name, sc.ClassName, ss.SubjectId })
                    .Join(db.Subjects, scss => scss.SubjectId, s => s.SubjectId, (scss, s) => new { scss.Name, scss.ClassName, s.SubjectName });

                var classes = new HashSet<string>();
                foreach (var m in marksAnonA)
                {
                    classes.Add(m.ClassName);
                }

                return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(), Subjects = new List<string>(), Marks = new List<byte?>() });
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        [HttpPost]
        public IActionResult AddMarkByForm(CustomTeacherMarks customTeacherMarks)
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            // Admin admin = HttpContext.Session.Get<Admin>("admin");
            var marksAnonA = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassId, c.ClassName, Name = s.FirstName + " " + s.LastName, s.StudentId })
                    .Join(db.StudentSchedules, sc => sc.ClassId, ss => ss.ClassId, (sc, ss) => new { sc.Name, sc.ClassName, ss.SubjectId, sc.StudentId })
                    .Join(db.Subjects, scss => scss.SubjectId, s => s.SubjectId, (scss, s) => new { scss.Name, scss.ClassName, s.SubjectName, scss.StudentId, s.SubjectId });
                var classes = new HashSet<string>();
                foreach (var m in marksAnonA)
                {
                    classes.Add(m.ClassName);
                }

                var marksAnonB = marksAnonA.Where(m => m.ClassName == customTeacherMarks.SelectedClass);
                var students = new HashSet<string>();
                foreach (var m in marksAnonB)
                {
                    students.Add(m.Name + " (" + m.StudentId + ")");
                }

                var marksAnonC = marksAnonB.Where(m => m.Name + " (" + m.StudentId + ")" == customTeacherMarks.SelectedStudent);
                var subjects = new HashSet<string>();
                foreach (var m in marksAnonC)
                {
                    subjects.Add(m.SubjectName);
                }

                var marks = new List<byte?>();
                if (customTeacherMarks.SelectedSubject != null)
                {
                    marks = new List<byte?>(new byte?[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                }

                if (customTeacherMarks.SelectedMark != null)
                {
                    int studendId = 0;
                    int subjectId = 0;
                    foreach (var m in marksAnonA)
                    {
                        if (customTeacherMarks.SelectedStudent == m.Name + " (" + m.StudentId + ")" && customTeacherMarks.SelectedSubject == m.SubjectName)
                        {
                            studendId = m.StudentId;
                            subjectId = m.SubjectId;
                        }
                    }

                    db.Database.ExecuteSqlInterpolated($"INSERT INTO Mark (Mark, StudentId, SubjectId) VALUES ({customTeacherMarks.SelectedMark}, {studendId}, {subjectId})");
                    return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(), Subjects = new List<string>(), Marks = new List<byte?>() });
                }

                return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(students), Subjects = new List<string>(subjects), Marks = new List<byte?>(marks) });
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
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

        public IActionResult AddLessonByForm()
        {
            //if (HttpContext.Session.Keys.Contains("admin"))
            //{
            var scheduleAnonA = db.Students
                .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassName });
            
            var classes = new HashSet<string>();
            foreach (var s in scheduleAnonA)
            {
                classes.Add(s.ClassName);
            }

            var subjects = new HashSet<string>();
            foreach (var s in db.Subjects)
            {
                subjects.Add(s.SubjectName);
            }

            return View(new CustomLessons {
                Classes = new List<string>(classes), 
                Subjects = new List<string>(subjects), 
                Teachers = new List<string>(), 
                DaysOfWeek = new List<byte?>(), 
                LessonNumbers = new List<byte?>(), 
                Cabinets = new List<int?>() 
            });
            //}
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        [HttpPost]
        public IActionResult AddLessonByForm(CustomLessons lessons)
        {
            var classesWithStudents = db.Students
                .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassName });
            var classes = new HashSet<string>();
            foreach (var c in classesWithStudents)
            {
                classes.Add(c.ClassName);
            }

            var subjects = new List<string>();
            foreach (var s in db.Subjects)
            {
                subjects.Add(s.SubjectName);
            }

            var daysOfTheWeek = new List<byte?>();
            var lessonNumbers = new List<byte?>();
            if (lessons.SelectedClass != null && lessons.SelectedSubject != null && lessons.SelectedCabinet == null)
            {
                daysOfTheWeek = new List<byte?>() { 1, 2, 3, 4, 5 };
                var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == lessons.SelectedClass);
                HttpContext.Session.Set("selectedclass", selectedClass);
                HttpContext.Session.Set("selectedsubject", lessons.SelectedSubject);
                var daysWithCounts = db.StudentSchedules.Where(ss => ss.ClassId == selectedClass.ClassId)
                .GroupBy(ss => ss.DayOfWeek)
                .Select(ss => new { ss.Key, Count = ss.Count() });
                foreach (var d in daysWithCounts)
                {
                    if (d.Count == 7)
                    {
                        daysOfTheWeek.Remove(d.Key);
                    }
                }
            }

            if (lessons.SelectedDayOfWeek != null && lessons.SelectedCabinet == null)
            {
                var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == lessons.SelectedClass);
                var daysWithCounts = db.StudentSchedules.Where(ss => ss.ClassId == selectedClass.ClassId)
                .GroupBy(ss => ss.DayOfWeek)
                .Select(ss => new { ss.Key, Count = ss.Count() });
                lessonNumbers = new List<byte?>() { 1, 2, 3, 4, 5, 6, 7 };
                for (byte i = 1; i <= 7; i++)
                {
                    foreach (var d in daysWithCounts)
                    {
                        if (d.Key == lessons.SelectedDayOfWeek && i <= d.Count)
                        {
                            lessonNumbers.Remove(i);
                        }
                    }
                }
            }

            var teachers = new List<string>();
            if (lessons.SelectedLessonNumber != null)
            {
                var teachs = db.Teachers.ToList();
                foreach (var t in teachs)
                {
                    bool isBusy = db.StudentSchedules.Any(ss => ss.TeacherId == t.TeacherId && ss.LessonNumber == lessons.SelectedLessonNumber &&
                        ss.DayOfWeek == lessons.SelectedDayOfWeek);
                    if (!isBusy)
                    {
                        teachers.Add(t.FirstName + " " + t.LastName + "(" + t.TeacherId + ")");
                    }
                }
            }

            var cabinets = new List<int?>();
            if (lessons.SelectedTeacher != null)
            {
                var cabs = db.Cabinets.ToList();
                foreach (var c in cabs)
                {
                    bool isBusy = db.StudentSchedules.Any(ss => ss.CabinetId == c.CabinetId &&
                        ss.DayOfWeek == lessons.SelectedDayOfWeek &&
                        ss.LessonNumber == lessons.SelectedLessonNumber);
                    if (!isBusy)
                    {
                        cabinets.Add(c.CabinetId);
                    }
                }
            }

            if (lessons.SelectedCabinet != null)
            {
                var selectedClass = HttpContext.Session.Get<Class>("selectedclass");
                var selectedSubject = HttpContext.Session.Get<string>("selectedsubject");
                HttpContext.Session.Remove("selectedclass");
                HttpContext.Session.Remove("selectedsubject");
                int subjectId = 0;
                int teacherId = 0;
                foreach (var t in db.Teachers)
                {
                    if (lessons.SelectedTeacher == t.FirstName + " " + t.LastName + "(" + t.TeacherId + ")")
                    {
                        teacherId = t.TeacherId;
                    }
                }

                foreach (var s in db.Subjects)
                {
                    if (selectedSubject == s.SubjectName)
                    {
                        subjectId = s.SubjectId;
                    }
                }

                db.Database.ExecuteSqlInterpolated($"INSERT INTO StudentSchedule (LessonNumber, DayOfWeek, ClassId, CabinetId, SubjectId, TeacherId) VALUES ({lessons.SelectedLessonNumber}, {lessons.SelectedDayOfWeek}, {selectedClass.ClassId}, {lessons.SelectedCabinet}, {subjectId}, {teacherId})");
                return View(new CustomLessons {
                    Classes = new List<string>(classes),
                    Subjects = new List<string>(subjects),
                    Teachers = new List<string>(),
                    Cabinets = new List<int?>(),
                    DaysOfWeek = new List<byte?>(),
                    LessonNumbers = new List<byte?>()
                });
            }

            return View(new CustomLessons { 
                Classes = new List<string>(classes), 
                Subjects = new List<string>(subjects), 
                Teachers = new List<string>(teachers), 
                Cabinets = new List<int?>(cabinets), 
                DaysOfWeek = new List<byte?>(daysOfTheWeek), 
                LessonNumbers = new List<byte?>(lessonNumbers) 
            });
            
            //else
            //{
            //    return RedirectToAction("Logout", "Account");
            //}
        }

        public IActionResult AddLessonByXml()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLessonByXml(IFormFile formFile)
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