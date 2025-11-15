using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Models;
using OnlineCoursess.Context;
using System;
using System.Linq;

namespace OnlineCourses.Controllers
{
    public class InstructorController : Controller
    {
        MyContext db = new MyContext();

        // -------------------------------------------------------------------
        // A. Registration (Register)
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Instructor instructor)
        {
            // Check if the model state is valid based on [Required], [Compare], etc.
            if (ModelState.IsValid)
            {
                // 1. Check for duplicate email
                if (db.Instructors.Any(i => i.Email == instructor.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(instructor);
                }

                // 2. Assign required properties for persistence
                instructor.PasswordHash = instructor.Password; // Copying password from the [NotMapped] field to PasswordHash
                instructor.DateJoined = DateTime.Now;

                // 3. Save to database
                db.Instructors.Add(instructor);
                db.SaveChanges();

                // Redirect to the login page after successful registration
                return RedirectToAction(nameof(Login));
            }
            // If validation fails, return the View with error messages
            return View(instructor);
        }

        // -------------------------------------------------------------------
        // B. Login
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Using the Instructor Model to receive email and password
        public IActionResult Login(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                // Find instructor by email and compare the password (Plain Text comparison - see security warning)
                var foundInstructor = db.Instructors.FirstOrDefault(i => i.Email == instructor.Email && i.PasswordHash == instructor.Password);

                if (foundInstructor != null)
                {
                    // 💡 Authentication Logic goes here (Issuing the Authentication Ticket/Cookie for the Instructor Role)

                    // Redirect to the instructor's dashboard
                    return RedirectToAction("Dashboard", "Instructor");
                }

                // Add a general error message if login fails
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
            return View(instructor);
        }
    }
}