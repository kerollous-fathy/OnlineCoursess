using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineCoursess.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedingDataV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CategoryId", "CreatedAt", "Description", "Duration", "InstructorId", "Level", "Price", "Title" },
                values: new object[] { 3, 1, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Foundational course covering Pandas, NumPy, and basic ML algorithms.", 30, 1, "Beginner", 99.00m, "Introduction to Data Science (Python)" });

            migrationBuilder.UpdateData(
                table: "Enrolls",
                keyColumn: "EnrollId",
                keyValue: 1,
                column: "Progress",
                value: 100);

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "InstructorId", "Biography", "Certification", "DateJoined", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "Phone", "ProfileImage" },
                values: new object[,]
                {
                    { 2, "Expert UI/UX Designer and Prototyping Specialist.", "Certified Figma Designer", new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "nora.mousa@platform.com", "Nora", "Mousa", "Adel", "hashed_nora", "01198765432", "/images/instructor/nora.jpg" },
                    { 3, "Seasoned Business Analyst and Project Manager.", "PMP Certified", new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "karim.fouad@platform.com", "Karim", "Fouad", "Yasser", "hashed_karim", "01555544433", "/images/instructor/karim.jpg" }
                });

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 1,
                column: "ContentUrl",
                value: "https://youtube.com/mvc-intro");

            migrationBuilder.InsertData(
                table: "LessonContents",
                columns: new[] { "LessonContentId", "ContentType", "ContentUrl", "Duration", "LessonId", "OrderIndex" },
                values: new object[] { 2, "PDF", "/files/mvc-slides.pdf", 5, 1, 2 });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "LessonId", "CourseId", "Duration", "OrderIndex", "Title" },
                values: new object[] { 2, 1, 60, 2, "Understanding Controllers and Views" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "TransactionId",
                value: "TRX1");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "Comment",
                value: "Excellent course structure!");

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "BirthDay", "DateJoined", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "Phone", "ProfileImage" },
                values: new object[] { 2, new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed.m@example.com", "Ahmed", "Mourad", "Khaled", "hashed_ahmed", "01000000000", "/images/student/ahmed.jpg" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CategoryId", "CreatedAt", "Description", "Duration", "InstructorId", "Level", "Price", "Title" },
                values: new object[,]
                {
                    { 2, 2, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Master modern design principles and prototyping for mobile and web.", 15, 2, "Advanced", 79.00m, "Advanced UI/UX Design with Figma" },
                    { 4, 3, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn SEO, content, and social media marketing techniques.", 10, 3, "Beginner", 39.99m, "Digital Marketing & SEO Strategies" },
                    { 5, 2, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comprehensive guide to vector graphics and illustration techniques.", 25, 2, "Intermediate", 55.00m, "Adobe Illustrator Mastery" }
                });

            migrationBuilder.InsertData(
                table: "Enrolls",
                columns: new[] { "EnrollId", "CompletedAt", "CourseId", "EnrolledAt", "Progress", "StudentId" },
                values: new object[,]
                {
                    { 3, null, 1, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, 2 },
                    { 4, null, 3, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 2 }
                });

            migrationBuilder.InsertData(
                table: "LessonContents",
                columns: new[] { "LessonContentId", "ContentType", "ContentUrl", "Duration", "LessonId", "OrderIndex" },
                values: new object[] { 3, "Video", "https://vimeo.com/controllers-guide", 25, 2, 1 });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "LessonId", "CourseId", "Duration", "OrderIndex", "Title" },
                values: new object[,]
                {
                    { 5, 3, 40, 1, "Setting up Python Environment" },
                    { 6, 3, 70, 2, "Data Cleaning with Pandas" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "CourseId", "PaymentDate", "Status", "StudentId", "TransactionId" },
                values: new object[] { 3, 49.99m, 1, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid", 2, "TRX3" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Comment", "CourseId", "CreatedAt", "Rating", "StudentId" },
                values: new object[] { 2, "Very clear explanation, highly recommend.", 1, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 });

            migrationBuilder.InsertData(
                table: "Enrolls",
                columns: new[] { "EnrollId", "CompletedAt", "CourseId", "EnrolledAt", "Progress", "StudentId" },
                values: new object[] { 2, null, 2, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 1 });

            migrationBuilder.InsertData(
                table: "LessonContents",
                columns: new[] { "LessonContentId", "ContentType", "ContentUrl", "Duration", "LessonId", "OrderIndex" },
                values: new object[] { 5, "Text", "/guides/python-setup.html", 10, 5, 1 });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "LessonId", "CourseId", "Duration", "OrderIndex", "Title" },
                values: new object[,]
                {
                    { 3, 2, 30, 1, "Figma Prototyping Basics" },
                    { 4, 2, 50, 2, "Advanced Component Creation" },
                    { 7, 4, 55, 1, "Intro to SEO and Keyword Research" },
                    { 8, 5, 45, 1, "Vector Basics and Tools" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "CourseId", "PaymentDate", "Status", "StudentId", "TransactionId" },
                values: new object[] { 2, 79.00m, 2, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid", 1, "TRX2" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Comment", "CourseId", "CreatedAt", "Rating", "StudentId" },
                values: new object[] { 3, "Good content but needs more practical examples.", 2, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 });

            migrationBuilder.InsertData(
                table: "LessonContents",
                columns: new[] { "LessonContentId", "ContentType", "ContentUrl", "Duration", "LessonId", "OrderIndex" },
                values: new object[,]
                {
                    { 4, "Video", "https://youtube.com/figma-proto", 15, 3, 1 },
                    { 6, "PDF", "https://coursematerials.com/seo-guide", 20, 7, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Enrolls",
                keyColumn: "EnrollId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrolls",
                keyColumn: "EnrollId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Enrolls",
                keyColumn: "EnrollId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "LessonId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "InstructorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "InstructorId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Enrolls",
                keyColumn: "EnrollId",
                keyValue: 1,
                column: "Progress",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 1,
                column: "ContentUrl",
                value: "https://youtube.com/intro-mvc");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "TransactionId",
                value: "TRX123456");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "Comment",
                value: "Great introduction to MVC!");
        }
    }
}
