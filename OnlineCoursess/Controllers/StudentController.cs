using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Models;
using OnlineCoursess.Context;
using System;
using System.Linq;

namespace OnlineCourses.Controllers
{
    public class StudentController : Controller
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
        public IActionResult Register(Student student)
        {
            // ModelState.IsValid checks [Required], [Compare], and [EmailAddress] attributes
            if (ModelState.IsValid)
            {
                // 1. Check for duplicate email
                if (db.Students.Any(s => s.Email == student.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(student);
                }

                // 2. Assign required properties for persistence
                student.PasswordHash = student.Password; // 👈 Copying password from the [NotMapped] field to PasswordHash
                student.DateJoined = DateTime.Now;

                // 3. Save to database
                db.Students.Add(student);
                db.SaveChanges();

                // Redirect to the login page after successful registration
                return RedirectToAction(nameof(Login));
            }
            // If validation fails, return the View with error messages
            return View(student);
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
        // Using the Student model to receive email and password
        public IActionResult Login(Student student)
        {
            if (ModelState.IsValid)
            {
                // Find student by email and compare the hash
                var foundStudent = db.Students.FirstOrDefault(s => s.Email == student.Email && s.PasswordHash == student.Password);

                if (foundStudent != null)
                {
                    // Redirect to the user's home dashboard
                    return RedirectToAction("Index", "Home");
                }

                // Add a general error message if login fails
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
            return View(student);
        }
    }
}