using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Models;
using OnlineCoursess.Context;
using OnlineCoursess.ViewModels;
using System.Security.Claims;

namespace OnlineCourses.Controllers
{
    public class PaymentController : Controller
    {
        MyContext db = new MyContext();

        // -------------------------------------------------------------------
        // 1. Checkout (GET) - لعرض صفحة الدفع
        // -------------------------------------------------------------------
        [HttpGet]
        public IActionResult Checkout(int courseId)
        {
            // 🛑 التحقق من الدخول أولاً
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Student");
            }

            // جلب تفاصيل الدورة (السعر) لإظهارها في صفحة الدفع
            var course = db.Courses.Find(courseId);
            if (course == null) return NotFound();

            // إرسال نموذج الدفع مع ID الدورة
            var model = new PaymentViewModel { CourseId = courseId };
            ViewData["CourseTitle"] = course.Title;
            ViewData["Price"] = course.Price;

            return View(model); // View/Payment/Checkout.cshtml
        }


        // -------------------------------------------------------------------
        // 2. ProcessPayment (POST) - معالجة بيانات الدفع الوهمية
        // -------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessPayment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int currentStudentId = int.Parse(userIdString);

                // 2. تسجيل العملية في جدول Payments
                var paymentRecord = new Payment
                {
                    StudentId = currentStudentId,
                    CourseId = model.CourseId,
                    Amount = db.Courses.Find(model.CourseId).Price,
                    PaymentDate = DateTime.Now,
                    TransactionId = Guid.NewGuid().ToString(), // مُعرف معاملة وهمي
                    Status = "Paid"
                };
                db.Payments.Add(paymentRecord);

                // 3. 🚀 إنشاء سجل الالتحاق الجديد (Enrollment)
                var newEnrollment = new Enroll
                {
                    StudentId = currentStudentId,
                    CourseId = model.CourseId,
                    EnrolledAt = DateTime.Now,
                    Progress = 0
                };
                db.Enrolls.Add(newEnrollment);

                // 4. حفظ سجل الدفع والالتحاق معًا
                db.SaveChanges();

                // 5. التوجيه مباشرة إلى صفحة مشاهدة المحتوى
                return RedirectToAction("ViewCourse", "Student", new { id = model.CourseId });
            }

            // إذا فشل التحقق (لأن بيانات الكارت غير صالحة مثلاً)، أعد عرض صفحة الدفع
            return View("Checkout", model);
        }
    }
}