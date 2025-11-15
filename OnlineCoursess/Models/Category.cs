namespace OnlineCourses.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Navigation
        public ICollection<Course> Courses { get; set; }
    }
}
