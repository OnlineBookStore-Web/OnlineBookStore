using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        // Navigation property
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
