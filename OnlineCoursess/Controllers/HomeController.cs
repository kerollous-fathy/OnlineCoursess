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

            // 1. جلب الدورات (FeaturedCourses)
            viewModel.FeaturedCourses = db.Courses
                // Include: لجلب بيانات المدرب (Instructor) في نفس الاستعلام
                .Include(c => c.Instructor)
                // Include: لجلب التقييمات (Reviews) لحساب المتوسط
                .Include(c => c.Reviews)
                .OrderByDescending(c => c.CreatedAt) // ترتيب حسب الأحدث
                .Take(6) // عرض أول 6 دورات
                .ToList();

            // 2. جلب المدربين (Top Instructors)
            viewModel.TopInstructors = db.Instructors
                .Take(4)
                .ToList();

            // إرسال سلة البيانات (الـ ViewModel) إلى الـ View
            return View(viewModel);
        }
    }
}