using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OnlineBookStore.Models
{
    public class CartItem
    {
        [Key]
        public int BookID { get; set; }
        public string BookTitle { get; set; }
        [Required]
        public int UserID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        [ForeignKey("BookID")]
        public Book Book { get; set; }   // العلاقة بالـ Book
    }

    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal CartTotal { get; set; }
    }
}
