using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Author { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        // Navigation Property
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}