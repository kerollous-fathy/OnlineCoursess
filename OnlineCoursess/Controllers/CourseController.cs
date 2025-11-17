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
    }
}