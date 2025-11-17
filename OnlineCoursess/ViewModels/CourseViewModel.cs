using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursess.ViewModels
{
    public class CourseViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Course Title")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be positive")]
        [Display(Name = "Price ($)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration (Hours)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = "Beginner"; // Default

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; } = new List<SelectListItem>();
    }
}
