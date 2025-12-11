using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookID { get; set; }
        public virtual Book Book { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}