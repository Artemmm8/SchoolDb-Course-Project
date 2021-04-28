using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDbProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDbProject.Controllers
{
    public class StudentController : Controller
    {
        private SchoolDbContext db;

        public StudentController(SchoolDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Personal()
        {
            if (HttpContext.Session.Keys.Contains("student"))
            {
                Student student = HttpContext.Session.Get<Student>("student");
                return View(student);
            }
            else
            {
                var val = TempData.Get<Student>("studentik");
                if (val == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                HttpContext.Session.Set("student", val);
                return View(val);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Personal(Student student)
        {
            if (HttpContext.Session.Keys.Contains("student"))
            {
                if (ModelState.IsValid)
                {
                    Student studentFromSession = HttpContext.Session.Get<Student>("student");
                    studentFromSession.FirstName = student.FirstName;
                    studentFromSession.LastName = student.LastName;
                    studentFromSession.Email = student.Email;
                    studentFromSession.PhoneNumber = student.PhoneNumber;
                    db.Students.Update(studentFromSession);
                    db.SaveChanges();
                    HttpContext.Session.Set("student", studentFromSession);
                    return View(studentFromSession);
                }

                return View(student);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult Schedule()
        {
            if (HttpContext.Session.Keys.Contains("student"))
            {
                Student student = HttpContext.Session.Get<Student>("student");
                var schedule = db.StudentSchedules
                    .Where(ss => ss.ClassId == student.ClassId)
                    .Join(db.Subjects, ss => ss.SubjectId, s => s.SubjectId, (ss, s) => new { Subject = s.SubjectName, LessonNumber = ss.LessonNumber, DayOfWeek = ss.DayOfWeek, CabinetId = ss.CabinetId });
                List<CustomSchedule> customStudentSchedule = new List<CustomSchedule>();
                foreach (var s in schedule)
                {
                    customStudentSchedule.Add(new CustomSchedule { Subject = s.Subject, LessonNumber = s.LessonNumber, DayOfWeek = s.DayOfWeek, CabinetId = s.CabinetId });
                }

                return View(customStudentSchedule);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult Marks()
        {
            if (HttpContext.Session.Keys.Contains("student"))
            {
                Student student = HttpContext.Session.Get<Student>("student");
                var marksAnon = db.Students.Where(s => s.StudentId == student.StudentId)
                    .Join(db.Marks, s => s.StudentId, m => m.StudentId, (s, m) => new { Mark = m.Mark1, Subject = m.SubjectId })
                    .Join(db.Subjects, ms => ms.Subject, s => s.SubjectId, (ms, s) => new { ms.Mark, s.SubjectName });
                List<CustomMarks> customMarks = new List<CustomMarks>();
                List<byte?> marks = new List<byte?>();
                HashSet<string> subjects = new HashSet<string>();
                foreach (var m in marksAnon)
                {
                    subjects.Add(m.SubjectName);
                }

                foreach (var s in subjects)
                {
                    marks.Clear();
                    foreach (var m in marksAnon)
                    {
                        if (m.SubjectName == s)
                        {
                            marks.Add(m.Mark);
                        }
                    }

                    customMarks.Add(new CustomMarks { Mark = new List<byte?>(marks), Subject = s });
                }

                return View(customMarks);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }
    }
}
