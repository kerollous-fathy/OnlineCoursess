namespace OnlineCourses.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
