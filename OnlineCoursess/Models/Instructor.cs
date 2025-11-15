namespace OnlineCourses.Models
{
    public class Instructor
    {
        public int InstructorId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateJoined { get; set; }
        public string PasswordHash { get; set; }

        public string ProfileImage { get; set; }
        public string Biography { get; set; }
        public string Certification { get; set; }

        // Navigation
        public ICollection<Course> CreatedCourses { get; set; }
    }
}
