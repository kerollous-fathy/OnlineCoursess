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


            var instructors = new List<Instructor>()
            {
                new Instructor // Instructor 1 (Hassan)
                {
                    InstructorId = 1,
                    FirstName = "Hassan", MiddleName = "Ali", LastName = "Samir",
                    Email = "hassan.samir@platform.com", Phone = "01012345678",
                    DateJoined = fixedDate, PasswordHash = "hashed_pass_inst",
                    Biography = "Senior Developer and Certified Trainer.",
                    ProfileImage = "/images/instructor/hassan.jpg",
                    Certification = "Microsoft ASB.NET CORE MVC"
                },
                new Instructor // Instructor 2 (Nora)
                {
                    InstructorId = 2,
                    FirstName = "Nora", MiddleName = "Adel", LastName = "Mousa",
                    Email = "nora.mousa@platform.com", Phone = "01198765432",
                    DateJoined = fixedDate.AddDays(-10), PasswordHash = "hashed_nora",
                    Biography = "Expert UI/UX Designer and Prototyping Specialist.",
                    ProfileImage = "/images/instructor/nora.jpg",
                    Certification = "Certified Figma Designer"
                },
                new Instructor // Instructor 3 (Karim)
                {
                    InstructorId = 3,
                    FirstName = "Karim", MiddleName = "Yasser", LastName = "Fouad",
                    Email = "karim.fouad@platform.com", Phone = "01555544433",
                    DateJoined = fixedDate.AddDays(-5), PasswordHash = "hashed_karim",
                    Biography = "Seasoned Business Analyst and Project Manager.",
                    ProfileImage = "/images/instructor/karim.jpg",
                    Certification = "PMP Certified"
                }
            };
            modelBuilder.Entity<Instructor>().HasData(instructors);


            var students = new List<Student>()
            {
                new Student // Student 1 (Sara)
                {
                    StudentId = 1,
                    FirstName = "Sara", MiddleName = "Mohamed", LastName = "Ali",
                    Email = "sara.ali@example.com", Phone = "01234567890",
                    BirthDay = new DateTime(1998, 5, 10), DateJoined = fixedDate.AddDays(-30),
                    PasswordHash = "hashed_pass_std", ProfileImage = "/images/student/sara.jpg"
                },
                new Student // Student 2 (Ahmed)
                {
                    StudentId = 2,
                    FirstName = "Ahmed", MiddleName = "Khaled", LastName = "Mourad",
                    Email = "ahmed.m@example.com", Phone = "01000000000",
                    BirthDay = new DateTime(2001, 1, 1), DateJoined = fixedDate.AddDays(-10),
                    PasswordHash = "hashed_ahmed", ProfileImage = "/images/student/ahmed.jpg"
                }
            };
            modelBuilder.Entity<Student>().HasData(students);


            var courses = new List<Course>()
            {
                new Course // Course 1 (Programming)
                {
                    CourseId = 1, Title = "ASP.NET MVC Web Development", Description = "Build a complete web application using ASP.NET MVC and Entity Framework.",
                    Level = "Intermediate", Price = 49.99m, Duration = 20, CreatedAt = fixedDate.AddDays(-60),
                    CategoryId = 1, InstructorId = 1
                },
                new Course // Course 2 (Design)
                {
                    CourseId = 2, Title = "Advanced UI/UX Design with Figma", Description = "Master modern design principles and prototyping for mobile and web.",
                    Level = "Advanced", Price = 79.00m, Duration = 15, CreatedAt = fixedDate.AddDays(-30),
                    CategoryId = 2, InstructorId = 2
                },
                new Course // Course 3 (Programming)
                {
                    CourseId = 3, Title = "Introduction to Data Science (Python)", Description = "Foundational course covering Pandas, NumPy, and basic ML algorithms.",
                    Level = "Beginner", Price = 99.00m, Duration = 30, CreatedAt = fixedDate.AddDays(-10),
                    CategoryId = 1, InstructorId = 1
                },
                new Course // Course 4 (Business)
                {
                    CourseId = 4, Title = "Digital Marketing & SEO Strategies", Description = "Learn SEO, content, and social media marketing techniques.",
                    Level = "Beginner", Price = 39.99m, Duration = 10, CreatedAt = fixedDate.AddDays(-5),
                    CategoryId = 3, InstructorId = 3
                },
                new Course // Course 5 (Design)
                {
                    CourseId = 5, Title = "Adobe Illustrator Mastery", Description = "Comprehensive guide to vector graphics and illustration techniques.",
                    Level = "Intermediate", Price = 55.00m, Duration = 25, CreatedAt = fixedDate.AddDays(-2),
                    CategoryId = 2, InstructorId = 2
                }
            };
            modelBuilder.Entity<Course>().HasData(courses);


            var lessons = new List<Lesson>()
            {
                // Lesson 1 (for Course 1: ASP.NET MVC) - موجود سابقاً
                new Lesson { LessonId = 1, Title = "Introduction to MVC Structure", Duration = 45, OrderIndex = 1, CourseId = 1 },
                new Lesson { LessonId = 2, Title = "Understanding Controllers and Views", Duration = 60, OrderIndex = 2, CourseId = 1 },

                // Lessons for Course 2 (Advanced UI/UX Design - Instructor Nora)
                new Lesson { LessonId = 3, Title = "Figma Prototyping Basics", Duration = 30, OrderIndex = 1, CourseId = 2 },
                new Lesson { LessonId = 4, Title = "Advanced Component Creation", Duration = 50, OrderIndex = 2, CourseId = 2 },

                // Lessons for Course 3 (Data Science - Instructor Hassan)
                new Lesson { LessonId = 5, Title = "Setting up Python Environment", Duration = 40, OrderIndex = 1, CourseId = 3 },
                new Lesson { LessonId = 6, Title = "Data Cleaning with Pandas", Duration = 70, OrderIndex = 2, CourseId = 3 },
    
                // Lesson for Course 4 (Digital Marketing - Instructor Karim)
                new Lesson { LessonId = 7, Title = "Intro to SEO and Keyword Research", Duration = 55, OrderIndex = 1, CourseId = 4 },

                // Lesson for Course 5 (Illustrator - Instructor Nora)
                new Lesson { LessonId = 8, Title = "Vector Basics and Tools", Duration = 45, OrderIndex = 1, CourseId = 5 }
            };
            modelBuilder.Entity<Lesson>().HasData(lessons);


            var contents = new List<LessonContent>()
            {
                // Content for Lesson 1 (MVC Structure)
                new LessonContent {
                 LessonContentId = 1, LessonId = 1,
                 Title = "Module 1 Introduction Video", // 💡 إضافة العنوان
                 ContentUrl = "https://youtube.com/mvc-intro", ContentType = "Video", OrderIndex = 1, Duration = 15
                },

                new LessonContent {
                  LessonContentId = 2, LessonId = 1,
                  Title = "Setup Environment Checklist", // 💡 إضافة العنوان
                  ContentUrl = "/files/mvc-slides.pdf", ContentType = "PDF", OrderIndex = 2, Duration = 5
                },

                // Content for Lesson 2 (MVC Controllers)
                new LessonContent {
                LessonContentId = 3, LessonId = 2,
                Title = "Deep Dive into Controllers", // 💡 إضافة العنوان
                ContentUrl = "https://vimeo.com/controllers-guide", ContentType = "Video", OrderIndex = 1, Duration = 25
                },
    
                // Content for Lesson 3 (Figma Prototyping)
                new LessonContent {
                LessonContentId = 4, LessonId = 3,
                Title = "Figma Basics Demo", // 💡 إضافة العنوان
                ContentUrl = "https://youtube.com/figma-proto", ContentType = "Video", OrderIndex = 1, Duration = 15 
                },
    
                // Content for Lesson 5 (Python Setup)
                new LessonContent {
                LessonContentId = 5, LessonId = 5,
                Title = "Installation Guide (Text)", // 💡 إضافة العنوان
                ContentUrl = "/guides/python-setup.html", ContentType = "Text", OrderIndex = 1, Duration = 10 
                },
    
                // Content for Lesson 7 (SEO)
                new LessonContent {
                LessonContentId = 6, LessonId = 7,
                Title = "Keyword Research PDF Guide", // 💡 إضافة العنوان
                ContentUrl = "https://coursematerials.com/seo-guide", ContentType = "PDF", OrderIndex = 1, Duration = 20 
                }
            };
            modelBuilder.Entity<LessonContent>().HasData(contents);


            modelBuilder.Entity<Enroll>().HasData(
              new Enroll { EnrollId = 1, StudentId = 1, CourseId = 1, EnrolledAt = fixedDate.AddDays(-15), Progress = 100 },
              new Enroll { EnrollId = 2, StudentId = 1, CourseId = 2, EnrolledAt = fixedDate.AddDays(-5), Progress = 50 },
              new Enroll { EnrollId = 3, StudentId = 2, CourseId = 1, EnrolledAt = fixedDate.AddDays(-10), Progress = 80 },
              new Enroll { EnrollId = 4, StudentId = 2, CourseId = 3, EnrolledAt = fixedDate.AddDays(-1), Progress = 0 }
            );


            modelBuilder.Entity<Payment>().HasData(
               new Payment { PaymentId = 1, StudentId = 1, CourseId = 1, Amount = 49.99m, PaymentDate = fixedDate.AddDays(-15), TransactionId = "TRX1" },
               new Payment { PaymentId = 2, StudentId = 1, CourseId = 2, Amount = 79.00m, PaymentDate = fixedDate.AddDays(-5), TransactionId = "TRX2" },
               new Payment { PaymentId = 3, StudentId = 2, CourseId = 1, Amount = 49.99m, PaymentDate = fixedDate.AddDays(-10), TransactionId = "TRX3" }
            );


            modelBuilder.Entity<Review>().HasData(
               new Review { ReviewId = 1, StudentId = 1, CourseId = 1, Comment = "Excellent course structure!", CreatedAt = fixedDate.AddDays(-5), Rating = 5 },
               new Review { ReviewId = 2, StudentId = 2, CourseId = 1, Comment = "Very clear explanation, highly recommend.", CreatedAt = fixedDate.AddDays(-3), Rating = 4 },
               new Review { ReviewId = 3, StudentId = 1, CourseId = 2, Comment = "Good content but needs more practical examples.", CreatedAt = fixedDate.AddDays(-1), Rating = 3 }
            );

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
