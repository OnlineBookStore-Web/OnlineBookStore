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

        public int AuthorID { get; set; }
        public Authors? Author { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        [StringLength(500)]
        public string Description { get; set; }


        [Display(Name = "Book Cover")]
        public string ImageUrl { get; set; } = string.Empty;

        //many orders and many reviews
        public virtual List<OrderDetail>? OrderDetails { get; set; }

        public List<Review>? Reviews { get; set; }

        public int Sales { get; set; } = 0; // tracks popularity

        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}