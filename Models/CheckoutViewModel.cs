namespace OnlineBookStore.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }

        // Customer info
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
