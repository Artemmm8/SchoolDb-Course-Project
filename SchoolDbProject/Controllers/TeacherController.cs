using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolDbProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolDbProject.Controllers
{
    public class TeacherController : Controller
    {
        private SchoolDbContext db;

        public TeacherController(SchoolDbContext db)
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
            if (HttpContext.Session.Keys.Contains("teacher"))
            {
                Teacher teacher = HttpContext.Session.Get<Teacher>("teacher");
                return View(teacher);
            }
            else
            {
                var val = TempData.Get<Teacher>("teacherik");
                if (val == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                HttpContext.Session.Set("teacher", val);
                return View(val);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Personal(Teacher teacher)
        {
            if (HttpContext.Session.Keys.Contains("teacher"))
            {
                if (ModelState.IsValid)
                {
                    Teacher teacherFromSession = HttpContext.Session.Get<Teacher>("teacher");
                    teacherFromSession.FirstName = teacher.FirstName;
                    teacherFromSession.LastName = teacher.LastName;
                    teacherFromSession.Email = teacher.Email;
                    teacherFromSession.PhoneNumber = teacher.PhoneNumber;
                    db.Teachers.Update(teacherFromSession);
                    db.SaveChanges();
                    HttpContext.Session.Set("teacher", teacherFromSession);
                    return View(teacherFromSession);
                }

                return View(teacher);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult Schedule()
        {
            if (HttpContext.Session.Keys.Contains("teacher"))
            {
                Teacher teacher = HttpContext.Session.Get<Teacher>("teacher");
                var schedule = db.StudentSchedules
                    .Where(ss => ss.TeacherId == teacher.TeacherId)
                    .Join(db.Subjects, ss => ss.SubjectId, s => s.SubjectId, (ss, s) => new { Subject = s.SubjectName, LessonNumber = ss.LessonNumber, DayOfWeek = ss.DayOfWeek, CabinetId = ss.CabinetId });
                List<CustomSchedule> customSchedule = new List<CustomSchedule>();
                foreach (var s in schedule)
                {
                    customSchedule.Add(new CustomSchedule { Subject = s.Subject, LessonNumber = s.LessonNumber, DayOfWeek = s.DayOfWeek, CabinetId = s.CabinetId });
                }

                return View(customSchedule);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public IActionResult Marks()
        {
            if (HttpContext.Session.Keys.Contains("teacher"))
            {
                Teacher teacher = HttpContext.Session.Get<Teacher>("teacher");
                var marksAnonA = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassId, c.ClassName, Name = s.FirstName + " " + s.LastName })
                    .Join(db.StudentSchedules, sc => sc.ClassId, ss => ss.ClassId, (sc, ss) => new { sc.Name, sc.ClassName, ss.SubjectId, ss.TeacherId })
                    .Where(ss => ss.TeacherId == teacher.TeacherId)
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
        public IActionResult Marks(CustomTeacherMarks customMarks)
        {
            if (HttpContext.Session.Keys.Contains("teacher"))
            {
                Teacher teacher = HttpContext.Session.Get<Teacher>("teacher");
                var marksAnonA = db.Students
                    .Join(db.Classes, s => s.ClassId, c => c.ClassId, (s, c) => new { c.ClassId, c.ClassName, Name = s.FirstName + " " + s.LastName, s.StudentId })
                    .Join(db.StudentSchedules, sc => sc.ClassId, ss => ss.ClassId, (sc, ss) => new { sc.Name, sc.ClassName, ss.SubjectId, sc.StudentId, ss.TeacherId })
                    .Where(ss => ss.TeacherId == teacher.TeacherId)
                    .Join(db.Subjects, scss => scss.SubjectId, s => s.SubjectId, (scss, s) => new { scss.Name, scss.ClassName, s.SubjectName, scss.StudentId, s.SubjectId });
                var classes = new HashSet<string>();
                foreach (var m in marksAnonA)
                {
                    classes.Add(m.ClassName);
                }

                var marksAnonB = marksAnonA.Where(m => m.ClassName == customMarks.SelectedClass);
                var students = new HashSet<string>();
                foreach (var m in marksAnonB)
                {
                    students.Add(m.Name + " (" + m.StudentId + ")");
                }

                var marksAnonC = marksAnonB.Where(m => m.Name + " (" + m.StudentId + ")" == customMarks.SelectedStudent);
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
                        if (customMarks.SelectedStudent == m.Name + " (" + m.StudentId + ")" && customMarks.SelectedSubject == m.SubjectName)
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
    }
}
