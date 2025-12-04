using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class Books
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
    }
}
