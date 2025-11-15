using Microsoft.EntityFrameworkCore;
using OnlineCourses.Models;

namespace OnlineCoursess.Context
{
    public class MyContext : DbContext
    {
        // -------------------------------------------------------

        #region Tables 

        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Instructor> Instructors { get; set; } = default!;


        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Lesson> Lessons { get; set; } = default!;
        public DbSet<LessonContent> LessonContents { get; set; } = default!;


        public DbSet<Enroll> Enrolls { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
        #endregion

        // -------------------------------------------------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string connectionString = "Server=DESKTOP-OMMCE3P;Database=OnlineCourses;Trusted_Connection=True;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTime fixedDate = new DateTime(2025, 1, 1);

            var categories = new List<Category>()
            {
                new Category { CategoryId = 1, CategoryName = "Programming" },
                new Category { CategoryId = 2, CategoryName = "Design" },
                new Category { CategoryId = 3, CategoryName = "Business" }
            };
            modelBuilder.Entity<Category>().HasData(categories);


            var instructor = new Instructor
            {
                InstructorId = 1,
                FirstName = "Hassan",
                MiddleName = "Ali",
                LastName = "Samir",
                Email = "hassan.samir@platform.com",
                Phone = "01012345678",
                DateJoined = fixedDate,
                PasswordHash = "hashed_pass_inst",
                Biography = "Senior Developer and Certified Trainer.",
                ProfileImage = "/images/instructor/hassan.jpg",
                Certification = "Microsoft ASB.NET CORE MVC"
            };
            modelBuilder.Entity<Instructor>().HasData(instructor);


            var student = new Student
            {
                StudentId = 1,
                FirstName = "Sara",
                MiddleName = "Mohamed",
                LastName = "Ali",
                Email = "sara.ali@example.com",
                Phone = "01234567890",
                BirthDay = new DateTime(1998, 5, 10),
                DateJoined = fixedDate.AddDays(-30),
                PasswordHash = "hashed_pass_std",
                ProfileImage = "/images/student/sara.jpg"
            };
            modelBuilder.Entity<Student>().HasData(student);


            var course = new Course
            {
                CourseId = 1,
                Title = "ASP.NET MVC Web Development",
                Description = "Build a complete web application using ASP.NET MVC and Entity Framework.",
                Level = "Intermediate",
                Price = 49.99m,
                Duration = 20, // 20 Hours
                CreatedAt = fixedDate.AddDays(-60),
                CategoryId = 1,
                InstructorId = 1
            };
            modelBuilder.Entity<Course>().HasData(course);


            var lesson = new Lesson
            {
                LessonId = 1,
                Title = "Introduction to MVC Structure",
                Duration = 45,
                OrderIndex = 1,
                CourseId = 1
            };
            modelBuilder.Entity<Lesson>().HasData(lesson);


            var content = new LessonContent
            {
                LessonContentId = 1,
                LessonId = 1,
                ContentUrl = "https://youtube.com/intro-mvc",
                ContentType = "Video",
                OrderIndex = 1,
                Duration = 15
            };
            modelBuilder.Entity<LessonContent>().HasData(content);


            var enroll = new Enroll
            {
                EnrollId = 1,
                StudentId = 1,
                CourseId = 1,
                EnrolledAt = fixedDate.AddDays(-15),
                Progress = 25
            };
            modelBuilder.Entity<Enroll>().HasData(enroll);


            var payment = new Payment
            {
                PaymentId = 1,
                StudentId = 1,
                CourseId = 1,
                Amount = 49.99m,
                PaymentDate = fixedDate.AddDays(-15),
                TransactionId = "TRX123456"
            };
            modelBuilder.Entity<Payment>().HasData(payment);


            var review = new Review
            {
                ReviewId = 1,
                StudentId = 1,
                CourseId = 1,
                Comment = "Great introduction to MVC!",
                CreatedAt = fixedDate.AddDays(-5),
                Rating = 5
            };
            modelBuilder.Entity<Review>().HasData(review);

            // ------------------------------------------------------------------


            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.CreatedCourses)
                .HasForeignKey(c => c.InstructorId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
