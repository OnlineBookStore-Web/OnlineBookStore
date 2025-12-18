using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineBookStore.Controllers
{
    public class OrdersController : Controller
    {
        // قائمة ثابتة لتجربة frontend بدون DB
        private static List<Order> Orders = new List<Order>();

        // ============================
        // Checkout Page
        // GET: /Orders/Checkout
        // ============================
        [HttpGet]
        public IActionResult Checkout()
        {
            return View("~/Views/Orders/Checkout.cshtml");
        }

        // ============================
        // Place Order
        // POST: /Orders/PlaceOrder
        // ============================
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderDTO order)
        {
            if (order == null || order.Items == null || order.Items.Count == 0)
            {
                return Json(new { success = false, message = "Cart is empty!" });
            }

            // إنشاء أوردر جديد للتجربة
            Orders.Add(new Order
            {
                OrderID = Orders.Count + 1,
                UserID = int.Parse(HttpContext.Session.GetString("UserID") ?? "0"),
                OrderDate = System.DateTime.Now,
                TotalAmount = order.Total,
                Status = "Pending",
                OrderDetails = order.Items.Select(i => new OrderDetail
                {
                    BookID = i.Id,
                    Quantity = i.Qty
                    // UnitPrice غير موجود في موديلك، ما نضيفش
                }).ToList()
            });

            return Json(new { success = true, message = "Order placed successfully!" });
        }

        // ============================
        // Order History
        // GET: /Orders/OrderHistory
        // ============================
        [HttpGet]
        public IActionResult OrderHistory()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserID") ?? "0");

            // جلب كل الطلبات الخاصة بالمستخدم
            var userOrders = Orders.Where(o => o.UserID == userId).ToList();

            // تحويل كل Order لـ List<OrderDetail> لو محتاجة تفاصيل كل منتج في الطلب
            var orderDetailsList = userOrders
                .SelectMany(o => o.OrderDetails) // OrderDetails مفروض تكون property في Order
                .ToList();

            return View("OrderHistory", orderDetailsList);
        }

        // ============================
        // Cancel Order
        // GET: /Orders/Cancel/5
        // ============================
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var order = Orders.FirstOrDefault(o => o.OrderID == id);

            if (order != null && order.Status == "Pending")
            {
                order.Status = "Cancelled";
            }

            return RedirectToAction("OrderHistory");
        }
    }

    // ============================
    // Models لتجربة الـ Frontend
    // ============================
    public class OrderDTO
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public List<CartItemDTO>? Items { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }   // مهم للعرض
        public string? Image { get; set; }   // مهم للعرض
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
}
