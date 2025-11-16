using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For .Include() and Eager Loading
using OnlineCourses.Models;
using OnlineCoursess.Context;
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

            return View(courses); // Passes the list to View/Course/Index.cshtml
        }

        // -------------------------------------------------------------------
        // 2. Details - Displays a single Course's detailed page
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Details(int id)
        {
            // Eagerly load the course and all related content (Lessons, Content, Reviews)
            var course = db.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Contents) // Drill down to LessonContent
                .Include(c => c.Reviews)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound(); // Return 404 if the course ID is invalid
            }

            return View(course); // Passes the detailed Course Model to View/Course/Details.cshtml
        }

        // -------------------------------------------------------------------
        // 3. Enroll - Handles course enrollment (after assumed successful payment)
        // -------------------------------------------------------------------
        [HttpPost]
        public IActionResult Enroll(int courseId)
        {
            // 1. Authentication Check: Ensure the user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                // Redirects to the login page defined in Program.cs
                return RedirectToAction("Login", "Student");
            }

            // 2. 🔑 Retrieve Student ID from the Authentication Claim
            // We rely on StudentId being stored in ClaimTypes.NameIdentifier during Login
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate the extracted ID
            if (userIdString == null || !int.TryParse(userIdString, out int currentStudentId))
            {
                // If the identity is missing or corrupted
                return Unauthorized();
            }

            // 3. Check for duplicate enrollment (Business Logic)
            bool alreadyEnrolled = db.Enrolls.Any(e => e.CourseId == courseId && e.StudentId == currentStudentId);

            if (alreadyEnrolled)
            {
                // Return to the details page with a message if already enrolled
                return RedirectToAction(nameof(Details), new { id = courseId, message = "AlreadyEnrolled" });
            }

            // 4. Create new enrollment record
            var newEnrollment = new Enroll
            {
                StudentId = currentStudentId, // The ID extracted from the Claim
                CourseId = courseId,
                EnrolledAt = DateTime.Now,
                Progress = 0 // Start progress at zero
            };

            db.Enrolls.Add(newEnrollment);
            db.SaveChanges();

            // 5. Redirect to the content page to start learning
            return RedirectToAction("ViewCourse", "Student", new { id = courseId });
        }
    }
}