namespace OnlineCourses.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int InstructorId { get; set; } 
        public Instructor Instructor { get; set; }

        // Navigation
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Enroll> Enrolls { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Payment> Payments { get; set; }

    }
}
