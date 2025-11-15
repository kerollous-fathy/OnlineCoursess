namespace OnlineCourses.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public string Title { get; set; }
        public int Duration { get; set; }
        public int OrderIndex { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Navigation
        public ICollection<LessonContent> Contents { get; set; }

    }
}
