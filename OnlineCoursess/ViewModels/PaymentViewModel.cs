using System.ComponentModel.DataAnnotations;

namespace OnlineCoursess.ViewModels
{
    public class PaymentViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Card Number is required")]
        public string CardNumber { get; set; } = default!;

        [Required(ErrorMessage = "Card Holder Name is required")]
        public string CardHolderName { get; set; } = default!;

        [Required(ErrorMessage = "Expiry Date is required")]
        public string ExpiryDate { get; set; } = default!; // MM/YY

        [Required(ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; } = default!;
    }
}
