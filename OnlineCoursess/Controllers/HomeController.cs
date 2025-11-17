using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.ViewModels;
using OnlineCoursess.Context;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCourses.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();

        public IActionResult Index()
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}