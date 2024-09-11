using System.ComponentModel.DataAnnotations;

namespace TypicalTechTools.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [RegularExpression(@"^\d{1,18}(\.\d{2})?$", ErrorMessage = "Invalid product price format")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        [MinLength(25, ErrorMessage = "Product description must be at least 25 characters")]
        public string ProductDescription { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
