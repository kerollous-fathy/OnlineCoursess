namespace OnlineCourses.Models
{
    public class LessonContent
    {
        public int LessonContentId { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public string ContentUrl { get; set; }
        public string ContentType { get; set; }
        public int OrderIndex { get; set; }
        public int Duration { get; set; }


    }
}
