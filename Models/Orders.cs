using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }  // Navigation Property لـ User

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        // Navigation Property
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}