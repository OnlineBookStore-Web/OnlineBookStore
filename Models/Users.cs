using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Full Name must contain at least 3  characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
          ErrorMessage = "Email format should be like example@example.com")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password should contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;

        // Navigation Property
        public virtual List<Order> Orders { get; set; }
    }
}