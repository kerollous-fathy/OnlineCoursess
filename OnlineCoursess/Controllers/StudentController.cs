using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Models;
using OnlineCourses.Models;
using OnlineCoursess.Context;
using OnlineCoursess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineCourses.Controllers
{
    public class StudentController : Controller
    {
        MyContext db = new MyContext();

        // -------------------------------------------------------------------
        // A. Registration (Register) - (unchanged)
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
            if (ModelState.IsValid)
            {
                if (db.Students.Any(s => s.Email == student.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(student);
                }

                student.PasswordHash = student.Password;
                student.DateJoined = DateTime.Now;

                db.Students.Add(student);
                db.SaveChanges();

                return RedirectToAction(nameof(Login));
            }
            return View(student);
        }

        // -------------------------------------------------------------------
        // B. Login (FIXED Model Mismatch)
        // -------------------------------------------------------------------

        [HttpGet]
        public IActionResult Login()
        {
            // 💡 يجب أن نرسل نموذج من نوع LoginViewModel
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // 🛑 التعديل الجوهري: استقبل LoginViewModel ليحل مشكلة Model Mismatch
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // 💡 تجاوزنا مشكلة الـ Validation الخاصة بحقول الاسم هنا
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and password must be provided.");
                return View(model);
            }

            // Find student by email and compare the hash
            var foundStudent = db.Students.FirstOrDefault(s => s.Email == model.Email && s.PasswordHash == model.Password);

            if (foundStudent != null)
            {
                // 1. Create Claims Identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, foundStudent.StudentId.ToString()),
                    new Claim(ClaimTypes.Name, foundStudent.FirstName),
                    new Claim(ClaimTypes.Role, "Student")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                // 2. Sign In and issue the authentication cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect to the user's home dashboard
                return RedirectToAction("Index", "Course");
            }

            // Add a general error message if login fails
            ModelState.AddModelError(string.Empty, "Invalid email or password.");

            // 💡 الإرجاع الصحيح: نرسل الـ ViewModel مرة أخرى
            return View(model);
        }

        // -------------------------------------------------------------------
        // C. View Course Content (ViewCourse)
        // -------------------------------------------------------------------

        [HttpGet]
        public IActionResult ViewCourse(int id)
        {
            // 1. Authentication Check: التأكد من تسجيل الدخول
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Student");
            }

            // 2. الحصول على ID الطالب الحالي
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !int.TryParse(userIdString, out int currentStudentId))
            {
                return Unauthorized();
            }

            // 3. Enrollment Check: التأكد من أن الطالب مشترك بالفعل
            bool isEnrolled = db.Enrolls.Any(e => e.CourseId == id && e.StudentId == currentStudentId);

            if (!isEnrolled)
            {
                // إذا لم يكن مشتركًا، يتم إرجاعه إلى صفحة التفاصيل (للاشتراك والدفع)
                return RedirectToAction("Details", "Course", new { id = id, message = "NotEnrolled" });
            }


            // 4. 📚 Eager Loading: جلب الدورة وعلاقاتها (الحل لخطأ NullReferenceException)
            var courseContent = db.Courses
                // 💡 جلب بيانات المدرب (Instructor)
                .Include(c => c.Instructor)

                // 💡 جلب قائمة الدروس (Lessons)
                .Include(c => c.Lessons)
                    // 💡 جلب محتوى كل درس (Contents)
                    .ThenInclude(l => l.Contents)

                // 💡 جلب الدورة المطلوبة
                .FirstOrDefault(c => c.CourseId == id);

            if (courseContent == null)
            {
                return NotFound();
            }
            return View(courseContent);
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyCourses()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentStudentId))
            {
                return RedirectToAction("Login", "Student");
            }

            // 🛑 التصحيح: يجب تطبيق Include مباشرة على db.Enrolls لجلب العلاقة أولاً

            var enrolledCourses = db.Enrolls
                .Where(e => e.StudentId == currentStudentId)

                // 1. Include Course, Instructor, Category, etc.
                // يجب أن نبدأ بتحميل كل ما نحتاجه من العلاقات الداخلية لـ Enroll
                .Include(e => e.Course) // Include the main Course object
                    .ThenInclude(c => c.Instructor) // Drill down to Instructor
                .Include(e => e.Course) // Include Course again
                    .ThenInclude(c => c.Category) // Drill down to Category
                .Include(e => e.Course) // Include Course again
                    .ThenInclude(c => c.Reviews) // Drill down to Reviews

                // 2. 💡 بعد تحميل كل شيء، نستخدم Select لاختيار Course فقط
                .Select(e => e.Course)
                .ToList();

            ViewData["Title"] = "كورساتي المشترك بها";
            ViewBag.ActiveTab = "Enrolled"; // highlight Enrolled tab

            // 3. نستخدم نفس الـ View لعرض الكتالوج
            return View("~/Views/Course/Index.cshtml", enrolledCourses);
        }
    }
}
