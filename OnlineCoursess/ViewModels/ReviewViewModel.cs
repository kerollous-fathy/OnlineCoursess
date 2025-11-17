using System.ComponentModel.DataAnnotations;

namespace OnlineCoursess.ViewModels
{
    public class ReviewViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Rating (Stars) is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}
