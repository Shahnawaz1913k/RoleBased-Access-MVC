using System.ComponentModel.DataAnnotations;

namespace RoleBased.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name="Username")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name="Role")]
        public string Role { get; set; } = "Manager"; 
    }
}
