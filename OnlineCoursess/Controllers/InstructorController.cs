using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Models;
using OnlineCoursess.Context;
using OnlineCoursess.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

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
        //-------------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // 💡 يجب أن تكون async Task<IActionResult>
        public async Task<IActionResult> Login(Instructor instructor)
        {
            // 🛑 تجاوز التحقق من ModelState لـ FirstName/LastName وغيرها
            if (string.IsNullOrEmpty(instructor.Email) || string.IsNullOrEmpty(instructor.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View(instructor);
            }

            var foundInstructor = db.Instructors.FirstOrDefault(i => i.Email == instructor.Email && i.PasswordHash == instructor.Password);

            if (foundInstructor != null)
            {
                // 1. 🔑 إنشاء Claims Identity (الهوية)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, foundInstructor.InstructorId.ToString()),
                    new Claim(ClaimTypes.Name, foundInstructor.FirstName),
                    new Claim(ClaimTypes.Role, "Instructor") // 🛑 تحديد الدور
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 2. 🚀 تسجيل الدخول وإصدار الكوكي
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redirect to the instructor's dashboard
                // 💡 التوجيه إلى Dashboard بعد تسجيل الدخول بنجاح
                return RedirectToAction("Dashboard", "Instructor");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(instructor);
        }
        //-------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "Instructor")] // حماية: مسموح للمدربين فقط
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Instructor Dashboard";
            return View();
        }


        // -------------------------------------------------------------------
        // D. CreateCourse (GET) - لعرض نموذج إنشاء دورة
        // -------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public IActionResult CreateCourse()
        {
            var categories = db.Categories.ToList();
            var viewModel = new CourseViewModel
            {
                CategoriesList = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
            };

            return View(viewModel); // توجيه إلى View/Instructor/CreateCourse.cshtml
        }

        // -------------------------------------------------------------------
        // E. CreateCourse (POST) - لحفظ بيانات الدورة
        // -------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public IActionResult CreateCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var instructorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (instructorIdString == null) return Unauthorized();

                int currentInstructorId = int.Parse(instructorIdString);

                var newCourse = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    Duration = model.Duration,
                    Level = model.Level,
                    CategoryId = model.CategoryId,
                    InstructorId = currentInstructorId, // ربط الدورة بالمدرب المسجل دخوله
                    CreatedAt = DateTime.Now
                };

                db.Courses.Add(newCourse);
                db.SaveChanges();

                // التوجيه لصفحة لوحة التحكم
                return RedirectToAction(nameof(Dashboard));
            }

            // إذا فشل التحقق، يجب إعادة ملء قائمة التصنيفات قبل إرجاع الـ View
            model.CategoriesList = db.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 💡 استخدام HttpContext.SignOutAsync لمسح ملف تعريف الارتباط (Cookie)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // التوجيه إلى الصفحة الرئيسية أو صفحة تسجيل الدخول بعد الخروج
            return RedirectToAction("Index", "Home");
        }
        //-------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "Instructor")] // حماية: يجب أن يكون المدرب مسجلاً للدخول
        public IActionResult MyCourses()
        {
            // 1. استخراج هوية المدرب الحالي
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out int currentInstructorId))
            {
                // إذا لم يكن ID صالحًا، أعده لصفحة الدخول
                return RedirectToAction("Login", "Instructor");
            }

            // 2. جلب جميع الدورات التي أنشأها هذا المدرب فقط
            var createdCourses = db.Courses
                .Where(c => c.InstructorId == currentInstructorId)
                .Include(c => c.Category)
                .Include(c => c.Reviews)
                .Include(c => c.Instructor)
                .ToList();

            ViewData["Title"] = "My Created Courses";

            // 3. نستخدم View الكتالوج المشترك (Views/Course/Index.cshtml) لعرض القائمة
            return View("MyCourses", createdCourses);
        }
        //-------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "Instructor")]
        public IActionResult EditCourse(int id)
        {
            // 1. الحصول على ID المدرب الحالي (لتحقيق الأمان)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentInstructorId))
            {
                return RedirectToAction("Login", "Instructor");
            }

            // 2. جلب الدورة والتحقق من ملكيتها
            var course = db.Courses
                .FirstOrDefault(c => c.CourseId == id && c.InstructorId == currentInstructorId);

            if (course == null)
            {
                // 🛑 إذا لم يتم العثور على الدورة أو لم يكن المدرب الحالي هو المالك
                return NotFound();
            }

            // 3. تحضير ViewModel بالبيانات الحالية وقائمة التصنيفات
            var categories = db.Categories.ToList();
            var viewModel = new CourseViewModel
            {
                // ملء الـ ViewModel ببيانات الدورة
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                Level = course.Level,
                CategoryId = course.CategoryId,

                // ملء قائمة التصنيفات لـ Dropdown List
                CategoriesList = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName,
                    Selected = (c.CategoryId == course.CategoryId) // تحديد التصنيف الحالي
                })
            };

            ViewData["CourseId"] = id; // تمرير ID الدورة
            ViewData["Title"] = "Edit Course: " + course.Title;
            return View(viewModel); // التوجيه إلى Views/Instructor/EditCourse.cshtml
        }
        //-------------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken] // حماية من هجمات CSRF
        [Authorize(Roles = "Instructor")] // حماية: مسموح للمدربين فقط
        public IActionResult EditCourse(int id, CourseViewModel model)
        {
            // 1. التحقق من ModelState (هل حقول النموذج صالحة؟)
            if (!ModelState.IsValid)
            {
                // إذا فشل التحقق، يجب إعادة ملء قائمة التصنيفات قبل إرجاع الـ View
                model.CategoriesList = db.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName,
                    Selected = (c.CategoryId == model.CategoryId)
                });

                // نعيد تمرير بيانات الـ ViewData التي يحتاجها الـ View
                ViewData["CourseId"] = id;
                ViewData["Title"] = "Edit Course: " + model.Title;
                return View(model);
            }

            // 2. التحقق من هوية المدرب الحالي (الأمان)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentInstructorId))
            {
                return RedirectToAction("Login", "Instructor");
            }

            // 3. جلب الكورس الحالي من قاعدة البيانات والتحقق من ملكيته (Ownership Check)
            var courseToUpdate = db.Courses
                .FirstOrDefault(c => c.CourseId == id && c.InstructorId == currentInstructorId);

            if (courseToUpdate == null)
            {
                // 🛑 إذا لم يتم العثور على الدورة أو لم يكن المدرب الحالي هو المالك، نمنع الوصول
                return NotFound();
            }

            // 4. تطبيق التعديلات من الـ ViewModel على الكورس
            courseToUpdate.Title = model.Title;
            courseToUpdate.Description = model.Description;
            courseToUpdate.Price = model.Price;
            courseToUpdate.Duration = model.Duration;
            courseToUpdate.Level = model.Level;
            courseToUpdate.CategoryId = model.CategoryId;
            // لا نغير InstructorId أو CreatedAt

            // 5. حفظ التعديلات في قاعدة البيانات
            db.SaveChanges();

            // 6. التوجيه إلى قائمة الدورات الخاصة بالمدرب
            return RedirectToAction(nameof(MyCourses));
        }
    }
}