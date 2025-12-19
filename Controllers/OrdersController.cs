using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OnlineBookStore.Controllers
{
    public class OrdersController : Controller
    {
        // قائمة مؤقتة (لو مش مستخدمة DB)
        private static List<Order> Orders = new List<Order>();

        private readonly AppDbContext _context;

        // ✅ Constructor لازم يبقى نفس اسم الـ Controller
        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // مثال Action
        public IActionResult Index()
        {
            return View(Orders);
        }

        
        // ============================
        // Checkout Page
        // GET: /Orders/Checkout
        // ============================
        [HttpGet]
       
            public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "Your cart is empty";
                return RedirectToAction("Index");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim);

            foreach (var item in cart)
            {
                var order = new Order
                {
                    UserID = userId,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("OrderHistory", "Orders");
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

            return View(userOrders);

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
