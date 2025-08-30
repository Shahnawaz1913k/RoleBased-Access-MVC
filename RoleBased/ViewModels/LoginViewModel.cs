using System.ComponentModel.DataAnnotations;

namespace RoleBased.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name="Remember me")]
        public bool RememberMe { get; set; }
    }
}
