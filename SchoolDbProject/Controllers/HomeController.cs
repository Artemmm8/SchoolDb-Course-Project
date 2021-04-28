using Microsoft.AspNetCore.Mvc;
using SchoolDbProject.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SchoolDbProject.Controllers
{
    public class HomeController : Controller
    {
        private SchoolDbContext db;

        public HomeController(SchoolDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}