using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace YourProject.Controllers
{
    public class OrdersController : Controller
    {
        // GET: /Orders/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            return View("~/Views/Orders/Checkout.cshtml");
        }

        // POST: /Orders/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderDTO order)
        {
            // === تجربة Frontend فقط ===
            // هنا ممكن تحطي الكود الحقيقي لحفظ الأوردر في قاعدة البيانات
            // مثال: Orders + OrderItems

            // للعرض دلوقتي نرجع Json رسالة نجاح
            return Json(new { success = true, message = "Order placed successfully!" });
        }
    }

    // Models لتجربة الـ Frontend
    public class OrderDTO
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<CartItemDTO> Items { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }   // مهم للعرض
        public string Image { get; set; }   // مهم للعرض
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
