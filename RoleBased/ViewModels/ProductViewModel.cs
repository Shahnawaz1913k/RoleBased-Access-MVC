using System.ComponentModel.DataAnnotations;

namespace RoleBased.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Product Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name="Price")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Enter valid price")]
        public decimal Price { get; set; }
    }
}
