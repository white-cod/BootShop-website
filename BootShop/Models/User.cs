using System.ComponentModel.DataAnnotations;

namespace BootShop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public Boolean IsAdmin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}