using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For .Include() and Eager Loading
using OnlineCourses.Models;
using OnlineCoursess.Context;
using OnlineCoursess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // To access User Identity (Claims)

namespace OnlineCourses.Controllers
{
    public class CourseController : Controller
    {
        MyContext db = new MyContext();

        // -------------------------------------------------------------------
        // 1. Index - Displays the Course Catalog
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Index()
        {
            // Fetch all courses with necessary related data (Instructor, Category, Reviews)
            var courses = db.Courses
                .Include(c => c.Instructor)  // Fetch Instructor details
                .Include(c => c.Category)    // Fetch Category name
                .Include(c => c.Reviews)     // Fetch Reviews (needed for rating calculation in View)
                .ToList();

            ViewBag.ActiveTab = "All"; // highlight All Courses tab
            return View(courses); // Passes the list to View/Course/Index.cshtml
        }

        // -------------------------------------------------------------------
        // 2. Details - Displays a single Course's detailed page
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Details(int id)
        {
            // Eagerly load the course and all related content (Lessons, Content, Reviews, Student Data)
            var course = db.Courses
                .Include(c => c.Instructor)  // جلب بيانات المدرب
                .Include(c => c.Category)    // جلب بيانات التصنيف
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Contents) // جلب محتويات كل درس
                .Include(c => c.Reviews)
                    .ThenInclude(r => r.Student) // 💡 إضافة هذا السطر: جلب بيانات الطالب الذي كتب التقييم
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound(); // Return 404 if the course ID is invalid
            }

            return View(course); // Passes the detailed Course Model to View/Course/Details.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitReview(ReviewViewModel model)
        {
            // 1. التحقق من المصادقة والـ Model State
            if (!User.Identity.IsAuthenticated || !ModelState.IsValid)
            {
                // إذا فشل التحقق، نعود لصفحة تفاصيل الدورة
                return RedirectToAction(nameof(Details), new { id = model.CourseId });
            }

            // 2. استخراج هوية الطالب
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentStudentId = int.Parse(userIdString);

            // 3. التحقق من أن الطالب لم يقم بالتقييم مسبقًا
            bool alreadyReviewed = db.Reviews.Any(r => r.CourseId == model.CourseId && r.StudentId == currentStudentId);

            if (alreadyReviewed)
            {
                // إذا قام بالتقييم بالفعل، أعده للصفحة
                return RedirectToAction(nameof(Details), new { id = model.CourseId, message = "AlreadyReviewed" });
            }

            // 4. إنشاء سجل التقييم
            var newReview = new Review
            {
                StudentId = currentStudentId,
                CourseId = model.CourseId,
                Rating = model.Rating,
                Comment = model.Comment,
                CreatedAt = DateTime.Now
            };

            db.Reviews.Add(newReview);
            db.SaveChanges();

            // 5. التوجيه مرة أخرى لصفحة التفاصيل (مع رسالة نجاح)
            return RedirectToAction(nameof(Details), new { id = model.CourseId, message = "ReviewSubmitted" });
        }
    }
}
