namespace OnlineCourses.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public string TransactionId { get; set; }
        public string Status { get; set; } = "Paid";

    }
}
