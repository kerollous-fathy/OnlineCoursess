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
            var viewModel = new LandingPageViewModel();

            viewModel.FeaturedCourses = db.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Reviews)
                .OrderByDescending(c => c.CreatedAt)
                .Take(6)
                .ToList();


            viewModel.TopInstructors = db.Instructors
                .Take(4)
                .ToList();


            return View(viewModel);
        }
    }
}