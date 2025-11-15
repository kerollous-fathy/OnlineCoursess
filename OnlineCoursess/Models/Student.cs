namespace OnlineCourses.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime DateJoined { get; set; }

        public string PasswordHash { get; set; }
        public string ProfileImage { get; set; }

        // Navigation
        public ICollection<Enroll> Enrolls { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Payment> Payments { get; set; }

    }
}
