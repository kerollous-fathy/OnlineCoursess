namespace OnlineCourses.Models
{
    public class Enroll
    {
        public int EnrollId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Progress { get; set; } // ratio of progress
    }
}
