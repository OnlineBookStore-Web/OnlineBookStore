using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class User
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

        // Navigation Property
        public virtual List<Order> Orders { get; set; }
    }
}