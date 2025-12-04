using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
