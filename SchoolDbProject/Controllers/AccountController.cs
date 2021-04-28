using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolDbProject.LoginAndRegistraionModels;
using SchoolDbProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System;
using System.Text.RegularExpressions;

namespace SchoolDbProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SchoolDbContext db;
        private readonly string passPath = @"F:\3-ий курс\SCHOOLDB COURSE PROJECT\PASSPATH\PASSPATH.txt";

        private async Task SendEmailAsync(string addressTo, bool isLogin)
        {
            MailAddress from = new MailAddress("schooldbproject@yandex.by", "School Project");
            MailAddress to = new MailAddress(addressTo);
            MailMessage m = new MailMessage(from, to);
            if (isLogin)
            {
                m.Subject = "Login";
                m.Body = "You have been successfully logged in in SchoolDbProject!";
            }
            else
            {
                m.Subject = "Register";
                m.Body = "You have been successfully registered in SchoolDbProject!";
            }

            string password;
            using (StreamReader reader = new StreamReader(passPath))
            {
                password = reader.ReadToEnd();
            }
            
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("schooldbproject@yandex.by", password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }

        public AccountController(SchoolDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentLogin(StudentLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = await db.Students.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
                if (student != null && VerifyHashedPassword(student.Password, model.Password))
                {
                    if (HttpContext.Session.Keys.Contains("student"))
                    {
                        await LogoutWithoutRedirect();
                        HttpContext.Session.Remove("student");
                    }

                    await Authenticate(model.StudentId.ToString());
                    if (model.StudentId == 8080)
                    {
                        TempData.Put("admin4ik", new Admin { AdminId = model.StudentId, Password = model.Password });
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        TempData.Put("studentik", student);
                        if (student.Email != null)
                        {
                            // SendEmailAsync(student.Email, true).GetAwaiter();
                        }

                        return RedirectToAction("Personal", "Student");
                    }
                }

                ViewBag.ErrorMessage = "Incorrect StudentId or Password.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult StudentRegister()
        {
            return View();
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return ByteArraysEqual(buffer3, buffer4);
        }

        private static bool ByteArraysEqual(byte[] buffer3, byte[] buffer4)
        {
            int count = 0;
            for (int i = 0; i < buffer3.Length; i++)
            {
                if (buffer3[i] == buffer4[i])
                {
                    count++;
                }
            }

            if (count == buffer4.Length)
            {
                return true;
            }

            return false;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentRegister(StudentRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = await db.Students.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
                if (student == null)
                {
                    if (HttpContext.Session.Keys.Contains("student"))
                    {
                        await LogoutWithoutRedirect();
                        HttpContext.Session.Remove("student");
                    }

                    Student s = new Student { StudentId = model.StudentId, Email = model.Email, Password = HashPassword(model.Password) };
                    db.Students.Add(s);
                    await db.SaveChangesAsync();
                    await Authenticate(model.StudentId.ToString());
                    TempData.Put("studentik", s);
                    if (s.Email != null)
                    {
                        // SendEmailAsync(s.Email, false).GetAwaiter();
                    }

                    return RedirectToAction("Personal", "Student");
                }
                else
                {
                    ViewBag.ErrorMessage = $"Student with Id = {student.StudentId} already exists.";
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult TeacherLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherLogin(TeacherLoginModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = await db.Teachers.FirstOrDefaultAsync(t => t.TeacherId == model.TeacherId);
                if (teacher != null && VerifyHashedPassword(teacher.Password, model.Password))
                {
                    if (HttpContext.Session.Keys.Contains("teacher"))
                    {
                        await LogoutWithoutRedirect();
                        HttpContext.Session.Remove("teacher");
                    }

                    await Authenticate(model.TeacherId.ToString());
                    if (model.TeacherId == 8080)
                    {
                        TempData.Put("admin4ik", new Admin { AdminId = model.TeacherId, Password = model.Password });
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        TempData.Put("teacherik", teacher);
                        if (teacher.Email != null)
                        {
                            // SendEmailAsync(teacher.Email, true).GetAwaiter();
                        }

                        return RedirectToAction("Personal", "Teacher");
                    }
                }

                ViewBag.ErrorMessage = "Incorrect TeacherId or Password.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult TeacherRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherRegister(TeacherRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = await db.Teachers.FirstOrDefaultAsync(t => t.TeacherId == model.TeacherId);
                if (teacher == null)
                {
                    if (HttpContext.Session.Keys.Contains("teacher"))
                    {
                        await LogoutWithoutRedirect();
                        HttpContext.Session.Remove("teacher");
                    }

                    Teacher t = new Teacher { TeacherId = model.TeacherId, Email = model.Email, Password = HashPassword(model.Password) };
                    db.Teachers.Add(t);
                    await db.SaveChangesAsync();
                    await Authenticate(model.TeacherId.ToString());
                    TempData.Put("teacherik", t);
                    if (t.Email != null)
                    {
                        // SendEmailAsync(t.Email, false).GetAwaiter();
                    }

                    return RedirectToAction("Personal", "Teacher");
                }
                else
                {
                    ViewBag.ErrorMessage = $"Teacher with Id = {teacher.TeacherId} already exists.";
                }
            }

            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task LogoutWithoutRedirect()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}