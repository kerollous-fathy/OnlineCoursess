using OnlineCourses.Models;
using System.Collections.Generic;

namespace OnlineCourses.ViewModels
{
    public class LandingPageViewModel
    {
        public List<Course> FeaturedCourses { get; set; } = new List<Course>();
        public List<Instructor> TopInstructors { get; set; } = new List<Instructor>();
    }
}