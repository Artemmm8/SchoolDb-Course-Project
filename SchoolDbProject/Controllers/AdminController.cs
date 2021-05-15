using SchoolDbProject.LoginAndRegistraionModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDbProject.Models;
using System.Threading;
using System.Linq;
using System.Xml;
using System.IO;
using System;

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

        public string ValidateXMLUsingXSD(string xmlPath, string xsdPath)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdPath));

                XmlReader xmlReader = XmlReader.Create(xmlPath, settings);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Authorize]
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
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

        // Finished
        #region Add By Form

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
                        ViewBag.ErrorMessage = $"Teacher with Id = { teacher.TeacherId} already exists.";
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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

                    db.Database.ExecuteSqlInterpolated($"INSERT INTO Mark (Mark, StudentId, SubjectId, Date) VALUES ({customMarks.SelectedMark}, {studendId}, {subjectId}, {DateTime.Now})");
                    return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(), Subjects = new List<string>(), Marks = new List<byte?>() });
                }

                return View(new CustomTeacherMarks { Classes = new List<string>(classes), Students = new List<string>(students), Subjects = new List<string>(subjects), Marks = new List<byte?>(marks) });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
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
                            teachers.Add(t.FirstName + " " + t.LastName + " (" + t.TeacherId + ")");
                        }
                    }
                }

                var cabinets = new List<int?>();
                if (!string.IsNullOrEmpty(lessons.SelectedTeacher))
                {
                    var cabs = db.Cabinets.ToList();
                    var selectedClass = HttpContext.Session.Get<Class>("selectedclass");
                    var set = db.StudentSchedules
                        .Join(db.Classes, ss => ss.ClassId, c => c.ClassId, (ss, c) => new { ss.CabinetId, ss.ClassId, ss.DayOfWeek, ss.LessonNumber, c.NumberOfStudents });
                    foreach (var c in cabs)
                    {
                        bool isBusy = set.Any(ss => ss.CabinetId == c.CabinetId &&
                            ss.DayOfWeek == lessons.SelectedDayOfWeek &&
                            ss.LessonNumber == lessons.SelectedLessonNumber);
                        if (!isBusy && selectedClass.NumberOfStudents <= c.NumberOfSeats)
                        {
                            cabinets.Add(c.CabinetId);
                        }
                    }
                }

                if (lessons.SelectedCabinet != null)
                {
                    var selectedClass = HttpContext.Session.Get<Class>("selectedclass");
                    var selectedTeacher = db.Teachers.FirstOrDefault(t => t.FirstName + " " + t.LastName + " (" + t.TeacherId + ")" == lessons.SelectedTeacher);
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

        #endregion Add By Form

        // Finished
        // TODO: Update add by xml. Check on null, empty, whitespace values in strings.
        #region Add By XML

        [Authorize]
        public IActionResult AddSubjectByXml()
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
        public IActionResult AddSubjectByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                    DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                    DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                    + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\subjects.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);

                    XmlNodeList dataNodes = doc.SelectNodes("/subjects/subject");
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        if (id <= 0)
                        {
                            ViewBag.Negative = $"Cannot insert object with negative Id ({id}). Only positive Id allowed.";
                            return View();
                        }

                        Subject subject = db.Subjects.FirstOrDefault(s => s.SubjectId == id);
                        if (subject != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate key. Duplicate key is {subject.SubjectId}. Import was interrupted.";
                            return View();
                        }

                        string pattern = "^[a-z]*$";
                        if (!Regex.IsMatch(item.SelectSingleNode("SubjectName").InnerText, pattern, RegexOptions.IgnoreCase))
                        {
                            ViewBag.NotOnlyLetters = $"Cannot insert object wtih incorrect subject name format ('{item.SelectSingleNode("SubjectName").InnerText}', key is {item.SelectSingleNode("SubjectId").InnerText}). Only letters allowed. Import was interrupted.";
                            return View();
                        }

                        subject = db.Subjects.FirstOrDefault(s => s.SubjectName == item.SelectSingleNode("SubjectName").InnerText); if (subject != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate name. Duplicate name is {subject.SubjectName}. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        string name = item.SelectSingleNode("SubjectName").InnerText;
                        db.Subjects.Add(new Subject { SubjectId = id, SubjectName = name });
                        db.SaveChanges();
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddClassByXml()
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
        public IActionResult AddClassByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\classes.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/classes/class");
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        if (id <= 0)
                        {
                            ViewBag.Negative = $"Cannot insert object with negative Id ({id}). Only positive Id allowed.";
                            return View();
                        }

                        Class @class = db.Classes.FirstOrDefault(c => c.ClassId == id);
                        if (@class != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate key. Duplicate key is {@class.ClassId}. Import was interrupted.";
                            return View();
                        }

                        string pattern = "[1-9]+[a-z]$";
                        var name = item.SelectSingleNode("ClassName").InnerText;
                        if (!(Regex.IsMatch(name, pattern, RegexOptions.IgnoreCase) && item.SelectSingleNode("ClassName").InnerText.Length <= 3))
                        {
                            ViewBag.NotOnlyLetters = $"Cannot insert object wtih incorrect class name format ('{item.SelectSingleNode("ClassName").InnerText}', key is {item.SelectSingleNode("ClassId").InnerText}). Name must consists of one or two numbers and letter. Import was interrupted.";
                            return View();
                        }

                        @class = db.Classes.FirstOrDefault(c => c.ClassName == name);
                        if (@class != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate name. Duplicate name is {@class.ClassName}. Import was interrupted.";
                            return View();
                        }

                        var num = Convert.ToInt32(item.SelectSingleNode("NumberOfStudents").InnerText);
                        if (!(num >= 0 && num <= 255))
                        {
                            ViewBag.Overflow = $"Cannot insert object with incorrect number of students value ({num}). Number must be in a range [0; 255]. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        string name = item.SelectSingleNode("ClassName").InnerText;
                        byte numberOfStudents = (byte)Convert.ToInt32(item.SelectSingleNode("NumberOfStudents").InnerText);
                        db.Classes.Add(new Class { ClassId = id, ClassName = name, NumberOfStudents = numberOfStudents });
                        db.SaveChanges();
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddCabinetByXml()
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
        public IActionResult AddCabinetByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                            DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                            DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                            + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\cabinets.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/cabinets/cabinet");
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        if (id <= 0)
                        {
                            ViewBag.Negative = $"Cannot insert object with negative Id ({id}). Only positive Id allowed.";
                            return View();
                        }

                        Cabinet cabinet = db.Cabinets.FirstOrDefault(c => c.CabinetId == id);
                        if (cabinet != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate key. Duplicate key is {cabinet.CabinetId}. Import was interrupted.";
                            return View();
                        }

                        var num = Convert.ToInt32(item.SelectSingleNode("NumberOfSeats").InnerText);
                        if (!(num >= 0 && num <= 255))
                        {
                            ViewBag.Overflow = $"Cannot insert object with incorrect number of seats value ({num}). Number must be in a range [0; 255]. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        byte numberOfSeats = (byte)Convert.ToInt32(item.SelectSingleNode("NumberOfSeats").InnerText);
                        db.Cabinets.Add(new Cabinet { CabinetId = id, NumberOfSeats = numberOfSeats });
                        db.SaveChanges();
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddTeacherByXml()
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
        public IActionResult AddTeacherByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\teachers.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/teachers/teacher");
                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("TeacherId").InnerText);
                        if (id <= 0)
                        {
                            ViewBag.Negative = $"Cannot insert object with negative Id ({id}). Only positive Id allowed.";
                            return View();
                        }

                        Teacher teacher = db.Teachers.FirstOrDefault(t => t.TeacherId == id);
                        if (teacher != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate key. Duplicate key is {teacher.TeacherId}. Import was interrupted.";
                            return View();
                        }

                        string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                        var email = item.SelectSingleNode("Email").InnerText;
                        if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                        {
                            ViewBag.EmailError = $"Cannot insert object wtih incorrect email format ('{item.SelectSingleNode("Email").InnerText}', key is {item.SelectSingleNode("TeacherId").InnerText}). Import was interrupted.";
                            return View();
                        }

                        // TODO: update regular expression.
                        // pattern = @"\+[0-9]{3}\([0-9]{2}\)[0-9]{3}-[0-9]{2}-[0-9]{2}";
                        pattern = @"[0-9]{3}[0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2}";
                        var number = item.SelectSingleNode("PhoneNumber").InnerText;
                        if (!Regex.IsMatch(number, pattern, RegexOptions.IgnoreCase))
                        {
                            ViewBag.PhoneError = $"Cannot insert object wtih incorrect phone number format ('{item.SelectSingleNode("PhoneNumber").InnerText}', key is {item.SelectSingleNode("TeacherId").InnerText}). Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("TeacherId").InnerText);
                        string fname = item.SelectSingleNode("FirstName").InnerText;
                        string lname = item.SelectSingleNode("LastName").InnerText;
                        string email = item.SelectSingleNode("Email").InnerText;
                        string pass = item.SelectSingleNode("Password").InnerText;
                        string phnum = item.SelectSingleNode("PhoneNumber").InnerText;
                        db.Teachers.Add(new Teacher { TeacherId = id, FirstName = fname, LastName = lname, Email = email, Password = pass, PhoneNumber = phnum });
                        db.SaveChanges();
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();

            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddStudentByXml()
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
        public IActionResult AddStudentByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\students.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/students/student");
                    var allClasses = db.Classes.ToList();
                    var classesAndPopulation = db.Students
                            .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { s.StudentId, c.ClassId })
                            .GroupBy(sc => sc.ClassId)
                            .Select(sc => new { sc.Key, Count = sc.Count() }).ToList();
                    foreach (var cp in classesAndPopulation)
                    {
                        var class1 = db.Classes.FirstOrDefault(c => c.ClassId == cp.Key);
                        if (cp.Count >= class1.NumberOfStudents)
                        {
                            allClasses.Remove(class1);
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("StudentId").InnerText);
                        if (id <= 0)
                        {
                            ViewBag.Negative = $"Cannot insert object with negative Id ({id}). Only positive Id allowed.";
                            return View();
                        }

                        Student student = db.Students.FirstOrDefault(s => s.StudentId == id);
                        if (student != null)
                        {
                            ViewBag.Duplicate = $"Cannot insert object with duplicate key. Duplicate key is {student.StudentId}. Import was interrupted.";
                            return View();
                        }

                        string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                        var email = item.SelectSingleNode("Email").InnerText;
                        if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                        {
                            ViewBag.EmailError = $"Cannot insert object wtih incorrect email format ('{item.SelectSingleNode("Email").InnerText}', key is {item.SelectSingleNode("StudentId").InnerText}). Import was interrupted.";
                            return View();
                        }

                        pattern = @"\+[0-9]{3}\([0-9]{2}\)[0-9]{3}-[0-9]{2}-[0-9]{2}";
                        var number = item.SelectSingleNode("PhoneNumber").InnerText;
                        if (!Regex.IsMatch(number, pattern, RegexOptions.IgnoreCase))
                        {
                            ViewBag.PhoneError = $"Cannot insert object wtih incorrect phone number format ('{item.SelectSingleNode("PhoneNumber").InnerText}', key is {item.SelectSingleNode("StudentId").InnerText}). Import was interrupted.";
                            return View();
                        }

                        var classid = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        if (classid <= 0)
                        {
                            ViewBag.NegativeClassId = $"Cannot insert object with not positive ClassId is ({classid}), key is {item.SelectSingleNode("StudentId").InnerText}. Import was interrupted.";
                            return View();
                        }

                        var class3 = db.Classes.FirstOrDefault(c => c.ClassId == classid);
                        if (class3 == null)
                        {
                            ViewBag.NotFounded = $"Cannot insert object in not founded class (key is {item.SelectSingleNode("ClassId").InnerText}). Student {item.SelectSingleNode("FirstName").InnerText} {item.SelectSingleNode("LastName").InnerText} cannot be added (key is {item.SelectSingleNode("StudentId").InnerText}). Import was interrupted.";
                            return View();
                        }

                        var class2 = allClasses.FirstOrDefault(c => c.ClassId == classid);
                        if (class2 == null)
                        {
                            ViewBag.ClassOverflow = $"Cannot insert object in limited set. Student {item.SelectSingleNode("FirstName").InnerText} {item.SelectSingleNode("LastName").InnerText} cannot be added to {db.Classes.FirstOrDefault(c => c.ClassId == classid).ClassName} class, because the number of students is already {db.Classes.FirstOrDefault(c => c.ClassId == classid).NumberOfStudents}. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        int id = Convert.ToInt32(item.SelectSingleNode("StudentId").InnerText);
                        string fname = item.SelectSingleNode("FirstName").InnerText;
                        string lname = item.SelectSingleNode("LastName").InnerText;
                        string email = item.SelectSingleNode("Email").InnerText;
                        string pass = item.SelectSingleNode("Password").InnerText;
                        string phnum = item.SelectSingleNode("PhoneNumber").InnerText;
                        int classid = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        db.Students.Add(new Student { StudentId = id, FirstName = fname, LastName = lname, Email = email, Password = pass, PhoneNumber = phnum, ClassId = classid });
                        db.SaveChanges();
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddMarkByXml()
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
        public IActionResult AddMarkByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\marks.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/marks/mark");
                    foreach (XmlNode item in dataNodes)
                    {
                        var mark = Convert.ToInt32(item.SelectSingleNode("Mark").InnerText);
                        if (mark <= 0 || mark >= 11)
                        {
                            ViewBag.WrongMark = $"Cannot insert object with incorrect mark value ({mark}), keys are {item.SelectSingleNode("StudentId").InnerText} (StudentId) and {item.SelectSingleNode("SubjectId").InnerText} (SubjectId). Allowed range is a [1; 10]. Import was interrupted.";
                            return View();
                        }

                        var stId = Convert.ToInt32(item.SelectSingleNode("StudentId").InnerText);
                        if (stId <= 0)
                        {
                            ViewBag.NegativeStudentId = $"Cannot insert object with not positive StudentId ({stId}). Import was interrupted.";
                            return View();
                        }

                        var student = db.Students.FirstOrDefault(s => s.StudentId == stId);
                        if (student == null)
                        {
                            ViewBag.NotFoundedStudent = $"Cannot insert object with not founded student, key is {stId}. Import was interrupted.";
                            return View();
                        }
                        else if (student.ClassId == null)
                        {
                            ViewBag.NotFoundedClass = $"Cannot insert object with not founded class. Student {student.FirstName} {student.LastName}, key is {student.StudentId}, is not related to any class. Import was interrupted.";
                            return View();
                        }

                        var subId = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        if (subId <= 0)
                        {
                            ViewBag.NegativeSubjectId = $"Cannot insert object with not positive SubjectId ({subId}). Import was interrupted.";
                            return View();
                        }

                        var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subId);
                        if (subject == null)
                        {
                            ViewBag.NotFoundedStudent = $"Cannot insert object with not founded subject, key is {subId}. Import was interrupted.";
                            return View();
                        }

                        var isExist = db.StudentSchedules.Any(ss => ss.SubjectId == subId && ss.ClassId == student.ClassId);
                        if (!isExist)
                        {
                            ViewBag.NoLessonForMarking = $"Cannot insert object with not founded lesson. There is no lesson for {db.Classes.FirstOrDefault(c => c.ClassId == student.ClassId).ClassName} class on {db.Subjects.FirstOrDefault(s => s.SubjectId == subId).SubjectName}. Import was interrupted.";
                            return View();
                        }

                        var dateString = item.SelectSingleNode("Date").InnerText;
                        if (!DateTime.TryParse(dateString, out DateTime dateValue))
                        {
                            ViewBag.BadDate = "Cannot insert object with incorrect date format. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        var mark = Convert.ToByte(item.SelectSingleNode("Mark").InnerText);
                        var stId = Convert.ToInt32(item.SelectSingleNode("StudentId").InnerText);
                        var subId = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        var date = Convert.ToDateTime(item.SelectSingleNode("Date").InnerText);
                        db.Database.ExecuteSqlInterpolated($"INSERT INTO Mark (Mark, StudentId, SubjectId, Date) VALUES ({mark}, {stId}, {subId}, {date})");
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult AddLessonByXml()
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
        public IActionResult AddLessonByXml(IFormFile file)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var xmlPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Files\" +
                        DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                        + file.FileName;
                var xsdPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\SchoolDbProject\SchoolDbProject\Schemas\lessons.xsd";
                if (Path.GetExtension(xmlPath) != ".xml")
                {
                    ViewBag.WrongExtension = "Sorry, cannot import data from this file. Only .xml extension allowed.";
                    return View();
                }

                using (var fileStream = new FileStream(xmlPath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    Thread.Sleep(2000);
                }

                string validate = ValidateXMLUsingXSD(xmlPath, xsdPath);
                if (validate != string.Empty)
                {
                    ViewBag.Validate = validate;
                    return View();
                }

                if (System.IO.File.Exists(xmlPath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlPath);
                    XmlNodeList dataNodes = doc.SelectNodes("/lessons/lesson");
                    foreach (XmlNode item in dataNodes)
                    {
                        var lesNum = Convert.ToInt32(item.SelectSingleNode("LessonNumber").InnerText);
                        if (lesNum <= 0 || lesNum >= 8)
                        {
                            ViewBag.WrongNumber = $"Cannot insert object with incorrect lesson number value ({lesNum}). Allowed range is a [1; 7]. Import was interrupted.";
                            return View();
                        }

                        var day = Convert.ToInt32(item.SelectSingleNode("DayOfWeek").InnerText);
                        if (day <= 0 || day >= 6)
                        {
                            ViewBag.WrongDay = $"Cannot insert object with incorrect day of week value ({day}). Allowed range is a [1; 5]. Import was interrupted.";
                            return View();
                        }

                        var clId = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        var @class = db.Classes.FirstOrDefault(c => c.ClassId == clId);
                        if (clId <= 0)
                        {
                            ViewBag.NegativeClassId = $"Cannot insert object with negative ClassId ({clId}). Only positive Id allowed. Import was interrupted.";
                            return View();
                        }

                        if (@class == null)
                        {
                            ViewBag.NotFoundedClass = $"Cannot insert object with not founded class, key is {clId}.Import was interrupted.";
                            return View();
                        }

                        var subId = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == subId);
                        if (subId <= 0)
                        {
                            ViewBag.NegativeSubjectId = $"Cannot insert object with negative SubjectId ({subId}). Only positive Id allowed. Import was interrupted.";
                            return View();
                        }

                        if (subject == null)
                        {
                            ViewBag.NotFoundedSubject = $"Cannot insert object with not founded subject, key is {subId}. Import was interrupted.";
                            return View();
                        }

                        var cabId = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        var cabinet = db.Cabinets.FirstOrDefault(c => c.CabinetId == cabId);
                        if (cabId <= 0)
                        {
                            ViewBag.NegativeCabinetId = $"Cannot insert object with negative CabinetId ({cabId}). Only positive Id allowed. Import was interrupted.";
                            return View();
                        }

                        if (cabinet == null)
                        {
                            ViewBag.NotFoundedCabinet = $"Cannot insert object with not founded cabinet, key is {cabId}. Import was interrupted.";
                            return View();
                        }

                        var teachId = Convert.ToInt32(item.SelectSingleNode("TeacherId").InnerText);
                        var teacher = db.Teachers.FirstOrDefault(t => t.TeacherId == teachId);
                        if (teachId <= 0)
                        {
                            ViewBag.NegativeTeacherId = $"Cannot insert object with negative TeacherId ({teachId}). Only positive Id allowed. Import was interrupted.";
                            return View();
                        }

                        if (teacher == null)
                        {
                            ViewBag.NotFoundedTeacher = $"Cannot insert object with not founded teacher, key is {teachId}. Import was interrupted.";
                            return View();
                        }

                        var isTeacherBusy = db.StudentSchedules.Any(ss => ss.DayOfWeek == day && ss.LessonNumber == lesNum && ss.TeacherId == teachId);
                        if (isTeacherBusy)
                        {
                            ViewBag.TeacherNotFree = $"Cannot insert object. Teacher {db.Teachers.FirstOrDefault(t => t.TeacherId == teachId).FirstName} {db.Teachers.FirstOrDefault(t => t.TeacherId == teachId).LastName}, key is {teachId}, is already busy. Import was interrupted.";
                            return View();
                        }

                        var isCabinetBusy = db.StudentSchedules.Any(ss => ss.DayOfWeek == day && ss.LessonNumber == lesNum && ss.CabinetId == cabId);
                        if (isCabinetBusy)
                        {
                            ViewBag.CabinetNotFree = $"Cannot insert object. Cabinet {cabId} is already busy. Import was interrupted.";
                            return View();
                        }

                        if (cabinet.NumberOfSeats < @class.NumberOfStudents)
                        {
                            ViewBag.CabinetOverflow = $"Cannot insert object. Number of seats in {cabId} cabinet is less than number of students in {@class.ClassName} class. Import was interrupted.";
                            return View();
                        }
                    }

                    foreach (XmlNode item in dataNodes)
                    {
                        var lesNum = Convert.ToByte(item.SelectSingleNode("LessonNumber").InnerText);
                        var day = Convert.ToByte(item.SelectSingleNode("DayOfWeek").InnerText);
                        var clId = Convert.ToInt32(item.SelectSingleNode("ClassId").InnerText);
                        var cabId = Convert.ToInt32(item.SelectSingleNode("CabinetId").InnerText);
                        var subId = Convert.ToInt32(item.SelectSingleNode("SubjectId").InnerText);
                        var teachId = Convert.ToInt32(item.SelectSingleNode("TeacherId").InnerText);
                        db.Database.ExecuteSqlInterpolated($"INSERT INTO StudentSchedule (LessonNumber, DayOfWeek, ClassId, CabinetId, SubjectId, TeacherId) VALUES ({lesNum}, {day}, {clId}, {cabId}, {subId}, {teachId})");
                    }
                }

                ViewBag.Success = "Import was finished successfully.";
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        #endregion Add By XML

        // Finished
        #region Delete

        [Authorize]
        public IActionResult DeleteSubject()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var subjects = new List<string>();
                foreach (var s in db.Subjects)
                {
                    subjects.Add(s.SubjectName);
                }

                return View(new CustomSubject { Subjects = subjects });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteSubject(CustomSubject subject)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var subjects = new List<string>();
                foreach (var s in db.Subjects)
                {
                    subjects.Add(s.SubjectName);
                }

                if (ModelState.IsValid)
                {
                    if (subject.SelectedSubject != null)
                    {
                        Subject subject1 = db.Subjects.FirstOrDefault(s => s.SubjectName == subject.SelectedSubject);
                        var isSubjectUsedInMarks = db.Marks.Any(m => m.SubjectId == subject1.SubjectId);
                        if (isSubjectUsedInMarks)
                        {
                            ViewBag.UsedInMarks = "Cannot delete this subject, because it is used in Marks.";
                            return View(new CustomSubject { Subjects = subjects });
                        }

                        var isSubjectUsedInLessons = db.StudentSchedules.Any(ss => ss.SubjectId == subject1.SubjectId);
                        if (isSubjectUsedInLessons)
                        {
                            ViewBag.UsedInLessons = "Cannot delete this subject, because it is used in Lessons.";
                            return View(new CustomSubject { Subjects = subjects });
                        }

                        if (subject1 != null)
                        {
                            db.Subjects.Remove(subject1);
                            db.SaveChanges();
                        }

                        return RedirectToAction("DeleteSubject", "Admin");
                    }
                }

                return View(new CustomSubject { Subjects = subjects });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteClass()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var classes = new List<string>();
                foreach (var c in db.Classes)
                {
                    classes.Add(c.ClassName);
                }

                return View(new CustomClass { Classes = classes });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteClass(CustomClass @class)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var classes = new List<string>();
                foreach (var c in db.Classes)
                {
                    classes.Add(c.ClassName);
                }

                if (ModelState.IsValid)
                {
                    if (@class.SelectedClass != null)
                    {
                        Class class1 = db.Classes.FirstOrDefault(c => c.ClassName == @class.SelectedClass);
                        var isClassUsedInStudents = db.Students.Any(s => s.ClassId == class1.ClassId);
                        if (isClassUsedInStudents)
                        {
                            ViewBag.UsedInStudents = "Cannot delete this class, because it is used in Students.";
                            return View(new CustomClass { Classes = classes });
                        }

                        var isClassUsedInLessons = db.StudentSchedules.Any(ss => ss.ClassId == class1.ClassId);
                        if (isClassUsedInLessons)
                        {
                            ViewBag.UsedInLessons = "Cannot delete this class, because it is used in Lessons.";
                            return View(new CustomClass { Classes = classes });
                        }

                        if (class1 != null)
                        {
                            db.Classes.Remove(class1);
                            db.SaveChanges();
                        }

                        return RedirectToAction("DeleteClass", "Admin");
                    }
                }

                return View(new CustomClass { Classes = classes });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteCabinet()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var cabinets = new List<int?>();
                foreach (var c in db.Cabinets)
                {
                    cabinets.Add(c.CabinetId);
                }

                return View(new CustomCabinet { Cabinets = cabinets });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteCabinet(CustomCabinet cabinet)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var cabinets = new List<int?>();
                foreach (var c in db.Cabinets)
                {
                    cabinets.Add(c.CabinetId);
                }

                if (ModelState.IsValid)
                {
                    if (cabinet.SelectedCabinet != null)
                    {
                        Cabinet cabinet1 = db.Cabinets.FirstOrDefault(c => c.CabinetId == cabinet.SelectedCabinet);
                        var isCabinetUsedInLessons = db.StudentSchedules.Any(ss => ss.CabinetId == cabinet1.CabinetId);
                        if (isCabinetUsedInLessons)
                        {
                            ViewBag.UsedInLessons = "Cannot delete this cabinet, because it is used in Lessons.";
                            return View(new CustomCabinet { Cabinets = cabinets });
                        }

                        if (cabinet1 != null)
                        {
                            db.Cabinets.Remove(cabinet1);
                            db.SaveChanges();
                        }

                        return RedirectToAction("DeleteCabinet", "Admin");
                    }
                }

                return View(new CustomCabinet { Cabinets = cabinets });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteTeacher()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var teachers = new List<string>();
                foreach (var t in db.Teachers)
                {
                    if (!string.IsNullOrEmpty(t.FirstName))
                    {
                        teachers.Add(t.FirstName + " " + t.LastName + " (" + t.TeacherId + ")");
                    }
                }

                return View(new CustomTeacher { Teachers = teachers });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteTeacher(CustomTeacher teacher)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var teachers = new List<string>();
                foreach (var t in db.Teachers)
                {
                    if (!string.IsNullOrEmpty(t.FirstName))
                    {
                        teachers.Add(t.FirstName + " " + t.LastName + " (" + t.TeacherId + ")");
                    }
                }

                if (ModelState.IsValid)
                {
                    if (teacher.SelectedTeacher != null)
                    {
                        Teacher teacher1 = db.Teachers.FirstOrDefault(t => t.FirstName + " " + t.LastName + " (" + t.TeacherId + ")" == teacher.SelectedTeacher);
                        var isTeacherUsedInLessons = db.StudentSchedules.Any(ss => ss.TeacherId == teacher1.TeacherId);
                        if (isTeacherUsedInLessons)
                        {
                            ViewBag.UsedInLessons = "Cannot delete this teacher, because it is used in Lessons.";
                            return View(new CustomTeacher { Teachers = teachers });
                        }

                        if (teacher1 != null)
                        {
                            db.Teachers.Remove(teacher1);
                            db.SaveChanges();
                        }

                        return RedirectToAction("DeleteTeacher", "Admin");
                    }
                }

                return View(new CustomTeacher { Teachers = teachers });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteStudent()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var students = new List<string>();
                foreach (var s in db.Students)
                {
                    if (!string.IsNullOrEmpty(s.FirstName))
                    {
                        students.Add(s.FirstName + " " + s.LastName + " (" + s.StudentId + ")");
                    }
                }

                return View(new CustomStudentDel { Students = students });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        // TODO: Trim names.
        [Authorize]
        [HttpPost]
        public IActionResult DeleteStudent(CustomStudentDel student)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var students = new List<string>();
                foreach (var s in db.Students)
                {
                    if (!string.IsNullOrEmpty(s.FirstName))
                    {
                        students.Add(s.FirstName + " " + s.LastName + " (" + s.StudentId + ")");
                    }
                }

                if (ModelState.IsValid)
                {
                    if (student.SelectedStudent != null)
                    {
                        Student student1 = db.Students.FirstOrDefault(s => s.FirstName + " " + s.LastName + " (" + s.StudentId + ")" == student.SelectedStudent);
                        var isStudentUsedInMarks = db.Marks.Any(m => m.StudentId == student1.StudentId);
                        if (isStudentUsedInMarks)
                        {
                            ViewBag.UsedInMarks = "Cannot delete this student, because it is used in Marks.";
                            return View(new CustomStudentDel { Students = students });
                        }

                        if (student1 != null)
                        {
                            db.Students.Remove(student1);
                            db.SaveChanges();
                        }

                        return RedirectToAction("DeleteStudent", "Admin");
                    }
                }

                return View(new CustomStudentDel { Students = students });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteMark()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var marks = new List<string>();
                var marksFriendly = db.Marks
                    .Join(db.Subjects, m => m.SubjectId, s => s.SubjectId, (m, s) => new { m.Mark1, m.StudentId, m.Date, s.SubjectName })
                    .Join(db.Students, ms => ms.StudentId, s => s.StudentId, (ms, s) => new { ms.Mark1, ms.Date, ms.SubjectName, s.FirstName, s.LastName, s.StudentId })
                    .ToList();
                foreach (var m in marksFriendly)
                {
                    if (!string.IsNullOrEmpty(m.FirstName))
                    {
                        marks.Add(m.Mark1 + "  " + m.SubjectName + "  " + m.FirstName + " " + m.LastName + " (" + m.StudentId + ")" + "  " + m.Date);
                    }
                }

                return View(new CustomMark { Marks = marks });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteMark(CustomMark mark)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var marks = new List<string>();
                var marksFriendly = db.Marks
                    .Join(db.Subjects, m => m.SubjectId, s => s.SubjectId, (m, s) => new { m.Mark1, m.StudentId, m.Date, s.SubjectName, s.SubjectId })
                    .Join(db.Students, ms => ms.StudentId, s => s.StudentId, (ms, s) => new { ms.Mark1, ms.Date, ms.SubjectName, s.FirstName, s.LastName, s.StudentId, ms.SubjectId })
                    .ToList();
                foreach (var m in marksFriendly)
                {
                    if (!string.IsNullOrEmpty(m.FirstName))
                    {
                        marks.Add(m.Mark1 + "  " + m.SubjectName + "  " + m.FirstName + " " + m.LastName + " (" + m.StudentId + ")" + "  " + m.Date);
                    }
                }

                if (ModelState.IsValid)
                {
                    if (mark.SelectedMark != null)
                    {
                        var mark1 = marksFriendly.FirstOrDefault(m => m.Mark1 + " " + m.SubjectName + " " + m.FirstName + " " + m.LastName + " (" + m.StudentId + ")" + " " + m.Date == mark.SelectedMark);
                        var mark2 = db.Marks.FirstOrDefault(m => m.StudentId == mark1.StudentId && m.SubjectId == mark1.SubjectId && m.Date == mark1.Date && m.Mark1 == mark1.Mark1);

                        if (mark2 != null)
                        {
                            db.Database.ExecuteSqlInterpolated($"DELETE FROM Mark WHERE Mark = {mark2.Mark1} AND StudentId = {mark2.StudentId} AND SubjectId = {mark2.SubjectId} AND Date = {mark2.Date}");
                        }

                        return RedirectToAction("DeleteMark", "Admin");
                    }
                }

                return View(new CustomMark { Marks = marks });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult DeleteLesson()
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var lessons = new List<string>();
                var days = new Dictionary<byte?, string>
            {
                { 1, "Monday" },
                { 2, "Tuesday"},
                { 3, "Wednesday" },
                { 4, "Thursday" },
                { 5, "Friday" }
            };

                var lessonsFriendly = db.StudentSchedules
                    .Join(db.Subjects, ss => ss.SubjectId, s => s.SubjectId, (ss, s) => new { ss.LessonNumber, Day = ss.DayOfWeek, ss.CabinetId, ss.TeacherId, ss.ClassId, s.SubjectName })
                    .Join(db.Classes, sssd => sssd.ClassId, c => c.ClassId, (sssd, c) => new { sssd.LessonNumber, sssd.Day, sssd.CabinetId, c.ClassName, sssd.TeacherId, sssd.SubjectName })
                    .Join(db.Teachers, sssdc => sssdc.TeacherId, t => t.TeacherId, (sssdc, t) => new { sssdc.LessonNumber, sssdc.Day, sssdc.CabinetId, sssdc.ClassName, sssdc.SubjectName, t.FirstName, t.LastName, t.TeacherId })
                    .ToList();
                foreach (var l in lessonsFriendly)
                {
                    foreach (var d in days)
                    {
                        if (!string.IsNullOrEmpty(l.FirstName) && d.Key == l.Day)
                        {
                            lessons.Add(d.Value + " " + l.LessonNumber + " " + l.ClassName + " " + l.SubjectName + " " + l.CabinetId + " " + l.FirstName + " " + l.LastName + " (" + l.TeacherId + ")");
                        }
                    }
                }

                return View(new CustomLesson { Lessons = lessons });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteLesson(CustomLesson lesson)
        {
            if (HttpContext.Session.Keys.Contains("admin"))
            {
                var lessons = new List<string>();
                var days = new Dictionary<byte?, string>
            {
                { 1, "Monday" },
                { 2, "Tuesday"},
                { 3, "Wednesday" },
                { 4, "Thursday" },
                { 5, "Friday" }
            };

                var lessonsFriendly = db.StudentSchedules
                    .Join(db.Subjects, ss => ss.SubjectId, s => s.SubjectId, (ss, s) => new { ss.LessonNumber, Day = ss.DayOfWeek, ss.CabinetId, ss.TeacherId, ss.ClassId, s.SubjectName, s.SubjectId })
                    .Join(db.Classes, sss => sss.ClassId, c => c.ClassId, (sss, c) => new { sss.LessonNumber, sss.Day, sss.CabinetId, c.ClassName, sss.TeacherId, sss.SubjectName, sss.SubjectId, c.ClassId })
                    .Join(db.Teachers, sssc => sssc.TeacherId, t => t.TeacherId, (sssc, t) => new { sssc.LessonNumber, sssc.Day, sssc.CabinetId, sssc.ClassName, sssc.SubjectName, t.FirstName, t.LastName, t.TeacherId, sssc.ClassId, sssc.SubjectId })
                    .ToList();
                foreach (var l in lessonsFriendly)
                {
                    foreach (var d in days)
                    {
                        if (!string.IsNullOrEmpty(l.FirstName) && d.Key == l.Day)
                        {
                            lessons.Add(d.Value + " " + l.LessonNumber + " " + l.ClassName + " " + l.SubjectName + " " + l.CabinetId + " " + l.FirstName + " " + l.LastName + " (" + l.TeacherId + ")");
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    if (lesson.SelectedLesson != null)
                    {
                        var words = lesson.SelectedLesson.Split(new char[] { ' ' });
                        switch (words[0])
                        {
                            case "Monday":
                                lesson.SelectedLesson = lesson.SelectedLesson.Replace(words[0], "1");
                                break;
                            case "Tuesday":
                                lesson.SelectedLesson = lesson.SelectedLesson.Replace(words[0], "2");
                                break;
                            case "Wednesday":
                                lesson.SelectedLesson = lesson.SelectedLesson.Replace(words[0], "3");
                                break;
                            case "Thursday":
                                lesson.SelectedLesson = lesson.SelectedLesson.Replace(words[0], "4");
                                break;
                            case "Friday":
                                lesson.SelectedLesson = lesson.SelectedLesson.Replace(words[0], "5");
                                break;
                        }

                        var lesson1 = lessonsFriendly.FirstOrDefault(l => l.Day + " " + l.LessonNumber + " " + l.ClassName + " " + l.SubjectName + " " + l.CabinetId + " " + l.FirstName + " " + l.LastName + " (" + l.TeacherId + ")" == lesson.SelectedLesson);
                        var lesson2 = db.StudentSchedules.FirstOrDefault(ss => ss.LessonNumber == lesson1.LessonNumber && ss.DayOfWeek == lesson1.Day && ss.ClassId == lesson1.ClassId && ss.SubjectId == lesson1.SubjectId && ss.CabinetId == lesson1.CabinetId && ss.TeacherId == lesson1.TeacherId);

                        if (lesson2 != null)
                        {
                            db.Database.ExecuteSqlInterpolated($"DELETE FROM StudentSchedule WHERE LessonNumber = {lesson2.LessonNumber} AND DayOfWeek = {lesson2.DayOfWeek} AND SubjectId = {lesson2.SubjectId} AND ClassId = {lesson2.ClassId} AND SubjectId = {lesson2.SubjectId} AND TeacherId = {lesson2.TeacherId}");
                        }

                        return RedirectToAction("DeleteLesson", "Admin");
                    }
                }

                return View(new CustomLesson { Lessons = lessons });
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        #endregion Delete
        // TODO: validating admin form.
        // NOT finished
        #region Edit
        #endregion Edit

        // Finished
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