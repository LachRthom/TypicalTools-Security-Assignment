using System.ComponentModel.DataAnnotations;

namespace TypicalTechTools.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        [Display(Name = "Comment")]
        [MinLength(25, ErrorMessage = "Comment text must be at least 25 characters")]
        public string CommentText { get; set; }

        [Display(Name = "Product Code")]
        public int ProductId { get; set; }

        public string? SessionId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
