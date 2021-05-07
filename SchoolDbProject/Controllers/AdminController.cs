﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                Admin admin = HttpContext.Session.Get<Admin>("admin");
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
        public IActionResult AddSubjectByForm()
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
        public IActionResult AddSubjectByForm(Subject subject)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                if (ModelState.IsValid)
                {
                    Subject subject1 = db.Subjects.FirstOrDefault(s => s.SubjectName == subject.SubjectName);
                    if (subject1 == null)
                    {
                        int id = db.Subjects.Max(s => s.SubjectId) + 1;
                        subject.SubjectId = id;
                        db.Subjects.Add(subject);
                        db.SaveChanges();
                        return RedirectToAction("AddSubjectByForm", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Subject with Name = {subject.SubjectName} already exists.";
                    }

                }
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddClassByForm()
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
        public IActionResult AddClassByForm(Class class1)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                if (ModelState.IsValid)
                {
                    Class @class = db.Classes.FirstOrDefault(c => c.ClassName == class1.ClassName);
                    if (@class == null)
                    {
                        int id = db.Classes.Max(c => c.ClassId) + 1;
                        class1.ClassId = id;
                        db.Classes.Add(class1);
                        db.SaveChanges();
                        return RedirectToAction("AddClassByForm", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Class with Class Name = {class1.ClassName} already exists.";
                    }
                }

                return View();

            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddCabinetByForm()
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
        public IActionResult AddCabinetByForm(Cabinet cabinet)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                if (ModelState.IsValid)
                {
                    var cab = db.Cabinets.FirstOrDefault(c => c.CabinetId == cabinet.CabinetId);
                    if (cab == null)
                    {
                        db.Cabinets.Add(cabinet);
                        db.SaveChanges();
                        return RedirectToAction("AddCabinetByForm", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Cabinet with Id = {cabinet.CabinetId} already exists.";
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddTeacherByForm()
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
        public IActionResult AddTeacherByForm(TeacherRegisterModel teacher)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                if (ModelState.IsValid)
                {
                    Teacher teacher1 = db.Teachers.FirstOrDefault(t => t.TeacherId == teacher.TeacherId);
                    if (teacher1 == null)
                    {
                        teacher.Password = AccountController.HashPassword(teacher.Password);
                        db.Teachers.Add(new Teacher
                        {
                            TeacherId = teacher.TeacherId,
                            Email = teacher.Email,
                            Password = teacher.Password
                        });

                        db.SaveChanges();
                        return RedirectToAction("AddTeacherByForm", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Teacher with Id = { teacher.TeacherId} already exists."; ;
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddStudentByForm()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                HashSet<string> classes = new HashSet<string>();
                var classesAndPopulation = db.Classes
                    .Join(db.Students, c => c.ClassId, s => s.ClassId, (c, s) => new { c.ClassName, s.StudentId })
                    .GroupBy(cs => cs.ClassName)
                    .Select(cs => new { cs.Key, Count = cs.Count() }).ToList();

                var allClasses = db.Classes.ToList();
                foreach (var cp in classesAndPopulation)
                {
                    var class1 = db.Classes.FirstOrDefault(c => c.ClassName == cp.Key);
                    if (cp.Count >= class1.NumberOfStudents)
                    {
                        allClasses.Remove(class1);
                    }
                }

                foreach (var c in allClasses)
                {
                    classes.Add(c.ClassName);
                }

                return View(new CustomStudent { Classes = new List<string>(classes) });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddStudentByForm(CustomStudent customStudent)
        {
            HashSet<string> classes = new HashSet<string>();
            var classesAndPopulation = db.Classes
                .Join(db.Students, c => c.ClassId, s => s.ClassId, (c, s) => new { c.ClassName, s.StudentId })
                .GroupBy(cs => cs.ClassName)
                .Select(cs => new { cs.Key, Count = cs.Count() }).ToList();

            var allClasses = db.Classes.ToList();
            foreach (var cp in classesAndPopulation)
            {
                var class1 = db.Classes.FirstOrDefault(c => c.ClassName == cp.Key);
                if (cp.Count >= class1.NumberOfStudents)
                {
                    allClasses.Remove(class1);
                }
            }

            foreach (var c in allClasses)
            {
                classes.Add(c.ClassName);
            }

            if (HttpContext.Session.Keys.Contains("admin"))
            {
                if (ModelState.IsValid)
                {
                    Student student1 = db.Students.FirstOrDefault(s => s.StudentId == customStudent.StudentId);
                    if (student1 == null)
                    {
                        customStudent.Password = AccountController.HashPassword(customStudent.Password);
                        Student student = new Student
                        {
                            StudentId = (int)customStudent.StudentId,
                            Email = customStudent.Email,
                            Password = customStudent.Password
                        };

                        int? classId = null;
                        if (customStudent.ClassName != null)
                        {
                            classId = db.Classes.FirstOrDefault(c => c.ClassName == customStudent.ClassName).ClassId;
                        }

                        student.ClassId = classId;
                        db.Students.Add(student);
                        db.SaveChanges();
                        return RedirectToAction("AddStudentByForm", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Student with Id = {customStudent.StudentId} already exists.";
                    }

                }

                return View(new CustomStudent { Classes = new List<string>(classes) });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddMarkByForm()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
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
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMarkByForm(CustomTeacherMarks customMarks)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var marksAnonA = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassId, c.ClassName, s.FirstName, s.LastName, s.StudentId })
                    .Join(db.StudentSchedules, sc => sc.ClassId, ss => ss.ClassId, (sc, ss) => new { sc.FirstName, sc.LastName, sc.ClassName, ss.SubjectId, sc.StudentId })
                    .Join(db.Subjects, scss => scss.SubjectId, s => s.SubjectId, (scss, s) => new { scss.FirstName, scss.LastName, scss.ClassName, s.SubjectName, scss.StudentId, s.SubjectId });
                var classes = new HashSet<string>();
                foreach (var m in marksAnonA)
                {
                    classes.Add(m.ClassName);
                }

                var marksAnonB = marksAnonA.Where(m => m.ClassName == customMarks.SelectedClass);
                var students = new HashSet<string>();
                foreach (var m in marksAnonB)
                {
                    if (!string.IsNullOrEmpty(m.FirstName))
                    {
                        students.Add(m.FirstName + " " + m.LastName + " (" + m.StudentId + ")");
                    }
                }

                var marksAnonC = marksAnonB.Where(m => m.FirstName + " " + m.LastName + " (" + m.StudentId + ")" == customMarks.SelectedStudent);
                var subjects = new HashSet<string>();
                foreach (var m in marksAnonC)
                {
                    subjects.Add(m.SubjectName);
                }

                var marks = new List<byte?>();
                if (customMarks.SelectedSubject != null)
                {
                    marks = new List<byte?>(new byte?[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                }

                if (customMarks.SelectedMark != null)
                {
                    int studendId = 0;
                    int subjectId = 0;
                    foreach (var m in marksAnonA)
                    {
                        if (customMarks.SelectedStudent == m.FirstName + " " + m.LastName + " (" + m.StudentId + ")" && customMarks.SelectedSubject == m.SubjectName)
                        {
                            studendId = m.StudentId;
                            subjectId = m.SubjectId;
                        }
                    }

                    db.Database.ExecuteSqlInterpolated($"INSERT INTO Mark (Mark, StudentId, SubjectId) VALUES ({customMarks.SelectedMark}, {studendId}, {subjectId})");
                    return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(), Subjects = new List<string>(), Marks = new List<byte?>() });
                }

                return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(students), Subjects = new List<string>(subjects), Marks = new List<byte?>(marks) });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        [Authorize]
        public IActionResult AddLessonByForm()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var classesWithStudents = db.Students
                .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassName });

                var classes = new HashSet<string>();
                foreach (var s in classesWithStudents)
                {
                    classes.Add(s.ClassName);
                }

                return View(new CustomLessons
                {
                    Classes = new List<string>(classes),
                    Subjects = new List<string>(),
                    Teachers = new List<string>(),
                    DaysOfWeek = new List<byte?>(),
                    LessonNumbers = new List<byte?>(),
                    Cabinets = new List<int?>()
                });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLessonByForm(CustomLessons lessons)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var classesWithStudents = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassName });
                var classes = new HashSet<string>();
                foreach (var c in classesWithStudents)
                {
                    classes.Add(c.ClassName);
                }

                if (!string.IsNullOrEmpty(lessons.SelectedClass) &&
                    string.IsNullOrEmpty(lessons.SelectedSubject) &&
                    lessons.SelectedDayOfWeek == null &&
                    lessons.SelectedLessonNumber == null &&
                    string.IsNullOrEmpty(lessons.SelectedTeacher) &&
                    lessons.SelectedCabinet == null)
                {
                    var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == lessons.SelectedClass);
                    HttpContext.Session.Set("selectedclass", selectedClass);
                }

                var subjects = new List<string>();
                if (!string.IsNullOrEmpty(lessons.SelectedClass))
                {
                    foreach (var s in db.Subjects)
                    {
                        subjects.Add(s.SubjectName);
                    }
                }

                var daysOfTheWeek = new List<byte?>();
                if (!string.IsNullOrEmpty(lessons.SelectedClass) && !string.IsNullOrEmpty(lessons.SelectedSubject) && lessons.SelectedCabinet == null)
                {
                    daysOfTheWeek = new List<byte?>() { 1, 2, 3, 4, 5 };
                    var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == lessons.SelectedClass);
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

                var lessonNumbers = new List<byte?>();
                if (lessons.SelectedDayOfWeek != null && lessons.SelectedCabinet == null)
                {
                    var selectedClass = db.Classes.FirstOrDefault(c => c.ClassName == lessons.SelectedClass);
                    var allLessonNumbers = new List<byte?>() { 1, 2, 3, 4, 5, 6, 7 };
                    var busyNumbersOfLessonsAnonType = db.StudentSchedules.Where(ss => ss.DayOfWeek == lessons.SelectedDayOfWeek &&
                        ss.ClassId == selectedClass.ClassId).Select(ss => new { ss.LessonNumber });
                    var busyNumbersOfLessons = new List<byte?>();
                    foreach (var l in busyNumbersOfLessonsAnonType)
                    {
                        busyNumbersOfLessons.Add(l.LessonNumber);
                    }

                    var lessonNumbers2 = allLessonNumbers.Except(busyNumbersOfLessons);
                    foreach (var l in lessonNumbers2)
                    {
                        lessonNumbers.Add(l);
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
                        if (!isBusy && !string.IsNullOrEmpty(t.FirstName))
                        {
                            teachers.Add(t.FirstName + " " + t.LastName + "(" + t.TeacherId + ")");
                        }
                    }
                }

                var cabinets = new List<int?>();
                if (!string.IsNullOrEmpty(lessons.SelectedTeacher))
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
                    var selectedTeacher = db.Teachers.FirstOrDefault(t => t.FirstName + " " + t.LastName + "(" + t.TeacherId + ")" == lessons.SelectedTeacher);
                    var selectedSubject = db.Subjects.FirstOrDefault(s => s.SubjectName == lessons.SelectedSubject);
                    db.Database.ExecuteSqlInterpolated($"INSERT INTO StudentSchedule (LessonNumber, DayOfWeek, ClassId, CabinetId, SubjectId, TeacherId) VALUES ({lessons.SelectedLessonNumber}, {lessons.SelectedDayOfWeek}, {selectedClass.ClassId}, {lessons.SelectedCabinet}, {selectedSubject.SubjectId}, {selectedTeacher.TeacherId})");
                    return View(new CustomLessons
                    {
                        Classes = new List<string>(classes),
                        Subjects = new List<string>(subjects),
                        Teachers = new List<string>(),
                        Cabinets = new List<int?>(),
                        DaysOfWeek = new List<byte?>(),
                        LessonNumbers = new List<byte?>()
                    });
                }

                return View(new CustomLessons
                {
                    Classes = new List<string>(classes),
                    Subjects = new List<string>(subjects),
                    Teachers = new List<string>(teachers),
                    Cabinets = new List<int?>(cabinets),
                    DaysOfWeek = new List<byte?>(daysOfTheWeek),
                    LessonNumbers = new List<byte?>(lessonNumbers)
                });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

        #region Export Data
        [Authorize]
        public FileContentResult ExportSubjects(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode subjectsNode = doc.CreateElement("subjects");
            doc.AppendChild(subjectsNode);
            foreach (var item in db.Subjects)
            {
                XmlNode subjectNode = doc.CreateElement("subject");
                subjectsNode.AppendChild(subjectNode);
                XmlNode idNode = doc.CreateElement("SubjectId");
                idNode.AppendChild(doc.CreateTextNode(item.SubjectId.ToString()));
                subjectNode.AppendChild(idNode);
                XmlNode nameNode = doc.CreateElement("SubjectName");
                nameNode.AppendChild(doc.CreateTextNode(item.SubjectName));
                subjectNode.AppendChild(nameNode);
            }

            doc.Save("subjects.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "subjects.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportClasses(string name)
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
                XmlNode numNode = doc.CreateElement("NumberOfStudents");
                numNode.AppendChild(doc.CreateTextNode(item.NumberOfStudents.ToString()));
                classNode.AppendChild(numNode);
            }

            doc.Save("classes.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "classes.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportCabinets(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode cabinetsNode = doc.CreateElement("cabinets");
            doc.AppendChild(cabinetsNode);
            foreach (var item in db.Cabinets)
            {
                XmlNode cabinetNode = doc.CreateElement("cabinet");
                cabinetsNode.AppendChild(cabinetNode);
                XmlNode idNode = doc.CreateElement("CabinetId");
                idNode.AppendChild(doc.CreateTextNode(item.CabinetId.ToString()));
                cabinetNode.AppendChild(idNode);
                XmlNode numNode = doc.CreateElement("NumberOfSeats");
                numNode.AppendChild(doc.CreateTextNode(item.NumberOfSeats.ToString()));
                cabinetNode.AppendChild(numNode);
            }

            doc.Save("cabinets.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "cabinets.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportTeachers(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode teachersNode = doc.CreateElement("teachers");
            doc.AppendChild(teachersNode);
            foreach (var item in db.Teachers)
            {
                XmlNode teacherNode = doc.CreateElement("teacher");
                teachersNode.AppendChild(teacherNode);

                XmlNode idNode = doc.CreateElement("TeacherId");
                idNode.AppendChild(doc.CreateTextNode(item.TeacherId.ToString()));
                teacherNode.AppendChild(idNode);

                XmlNode firstNameNode = doc.CreateElement("FirstName");
                firstNameNode.AppendChild(doc.CreateTextNode(item.FirstName));
                teacherNode.AppendChild(firstNameNode);

                XmlNode lastNameNode = doc.CreateElement("LastName");
                lastNameNode.AppendChild(doc.CreateTextNode(item.LastName));
                teacherNode.AppendChild(lastNameNode);

                XmlNode emailNode = doc.CreateElement("Email");
                emailNode.AppendChild(doc.CreateTextNode(item.Email));
                teacherNode.AppendChild(emailNode);

                XmlNode passNode = doc.CreateElement("Password");
                passNode.AppendChild(doc.CreateTextNode(item.Password));
                teacherNode.AppendChild(passNode);

                XmlNode phoneNode = doc.CreateElement("PhoneNumber");
                phoneNode.AppendChild(doc.CreateTextNode(item.PhoneNumber));
                teacherNode.AppendChild(phoneNode);
            }

            doc.Save("teachers.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "teachers.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportStudents(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode studentsNode = doc.CreateElement("students");
            doc.AppendChild(studentsNode);
            foreach (var item in db.Students)
            {
                XmlNode studentNode = doc.CreateElement("student");
                studentsNode.AppendChild(studentNode);

                XmlNode idNode = doc.CreateElement("StudentId");
                idNode.AppendChild(doc.CreateTextNode(item.StudentId.ToString()));
                studentNode.AppendChild(idNode);

                XmlNode firstNameNode = doc.CreateElement("FirstName");
                firstNameNode.AppendChild(doc.CreateTextNode(item.FirstName));
                studentNode.AppendChild(firstNameNode);

                XmlNode lastNameNode = doc.CreateElement("LastName");
                lastNameNode.AppendChild(doc.CreateTextNode(item.LastName));
                studentNode.AppendChild(lastNameNode);

                XmlNode emailNode = doc.CreateElement("Email");
                emailNode.AppendChild(doc.CreateTextNode(item.Email));
                studentNode.AppendChild(emailNode);

                XmlNode passNode = doc.CreateElement("Password");
                passNode.AppendChild(doc.CreateTextNode(item.Password));
                studentNode.AppendChild(passNode);

                XmlNode phoneNode = doc.CreateElement("PhoneNumber");
                phoneNode.AppendChild(doc.CreateTextNode(item.PhoneNumber));
                studentNode.AppendChild(phoneNode);

                XmlNode classIdNode = doc.CreateElement("ClassId");
                classIdNode.AppendChild(doc.CreateTextNode(item.ClassId.ToString()));
                studentNode.AppendChild(classIdNode);
            }

            doc.Save("students.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "students.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportMarks(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode marksNode = doc.CreateElement("marks");
            doc.AppendChild(marksNode);
            foreach (var item in db.Marks)
            {
                XmlNode markNode = doc.CreateElement("mark");
                marksNode.AppendChild(markNode);

                XmlNode markNumNode = doc.CreateElement("Mark");
                markNumNode.AppendChild(doc.CreateTextNode(item.Mark1.ToString()));
                markNode.AppendChild(markNumNode);

                XmlNode stIdNode = doc.CreateElement("StudentId");
                stIdNode.AppendChild(doc.CreateTextNode(item.StudentId.ToString()));
                markNode.AppendChild(stIdNode);

                XmlNode sbIdNode = doc.CreateElement("SubjectId");
                sbIdNode.AppendChild(doc.CreateTextNode(item.SubjectId.ToString()));
                markNode.AppendChild(sbIdNode);
            }

            doc.Save("marks.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "marks.xml"
            };

            return result;
        }

        [Authorize]
        public FileContentResult ExportLessons(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode lessonsNode = doc.CreateElement("lessons");
            doc.AppendChild(lessonsNode);
            foreach (var item in db.StudentSchedules)
            {
                XmlNode lessonNode = doc.CreateElement("lesson");
                lessonsNode.AppendChild(lessonNode);

                XmlNode lessonNumNode = doc.CreateElement("LessonNumber");
                lessonNumNode.AppendChild(doc.CreateTextNode(item.LessonNumber.ToString()));
                lessonNode.AppendChild(lessonNumNode);

                XmlNode dayNode = doc.CreateElement("DayOfWeek");
                dayNode.AppendChild(doc.CreateTextNode(item.DayOfWeek.ToString()));
                lessonNode.AppendChild(dayNode);

                XmlNode classIdNode = doc.CreateElement("ClassId");
                classIdNode.AppendChild(doc.CreateTextNode(item.ClassId.ToString()));
                lessonNode.AppendChild(classIdNode);

                XmlNode cabIdNode = doc.CreateElement("CabinetId");
                cabIdNode.AppendChild(doc.CreateTextNode(item.CabinetId.ToString()));
                lessonNode.AppendChild(cabIdNode);

                XmlNode sbIdNode = doc.CreateElement("SubjectId");
                sbIdNode.AppendChild(doc.CreateTextNode(item.SubjectId.ToString()));
                lessonNode.AppendChild(sbIdNode);

                XmlNode teachIdNode = doc.CreateElement("TeacherId");
                teachIdNode.AppendChild(doc.CreateTextNode(item.TeacherId.ToString()));
                lessonNode.AppendChild(teachIdNode);
            }

            doc.Save("lessons.xml");
            var data = System.IO.File.ReadAllBytes(name);
            var result = new FileContentResult(data, "application/octet-stream")
            {
                FileDownloadName = "lessons.xml"
            };

            return result;
        }
        #endregion Export Data
    }
}