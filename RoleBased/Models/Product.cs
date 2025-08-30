using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleBased.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string EncryptedPrice { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedByUserId { get; set; }
    }
}
