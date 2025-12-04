using System.ComponentModel.DataAnnotations;


namespace OnlineBookStore.Models
{
    public class OrdersDetails
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int BookID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
