using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoursess.Migrations
{
    /// <inheritdoc />
    public partial class AddContentTitles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LessonContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 1,
                column: "Title",
                value: "Module 1 Introduction Video");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 2,
                column: "Title",
                value: "Setup Environment Checklist");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 3,
                column: "Title",
                value: "Deep Dive into Controllers");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 4,
                column: "Title",
                value: "Figma Basics Demo");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 5,
                column: "Title",
                value: "Installation Guide (Text)");

            migrationBuilder.UpdateData(
                table: "LessonContents",
                keyColumn: "LessonContentId",
                keyValue: 6,
                column: "Title",
                value: "Keyword Research PDF Guide");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "LessonContents");
        }
    }
}
