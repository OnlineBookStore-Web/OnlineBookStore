using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OnlineBookStore.Helpers;


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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutViewModel
            {
                Items = cart,
                Total = cart.Sum(i => i.Total)
            };

            return View(vm);
        }




        // ============================
        // Place Order
        // POST: /Orders/PlaceOrder
        // ============================
        //[HttpPost]
        //public IActionResult PlaceOrder(string fullName, string address, string phone)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //        return RedirectToAction("Login", "Account");

        //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        //    if (cart == null || !cart.Any())
        //        return RedirectToAction("Index", "Cart");

        //    int userId = int.Parse(User.FindFirst(
        //        System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        //    var order = new Order
        //    {
        //        UserID = userId,
        //        OrderDate = DateTime.Now,
        //        Status = "Pending",
        //        TotalAmount = cart.Sum(i => i.Price * i.Quantity)
        //    };

        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    foreach (var item in cart)
        //    {
        //        _context.OrdersDetails.Add(new OrderDetail
        //        {
        //            OrderID = order.OrderID,
        //            BookID = item.BookID,
        //            Quantity = item.Quantity,
        //            Price = item.Price
        //        });
        //    }

        //    _context.SaveChanges();

        //    // 🧹 Clear cart after success
        //    HttpContext.Session.Remove("Cart");

        //    return RedirectToAction("OrderHistory");
        //}

        [HttpPost]
        //public IActionResult PlaceOrder(CheckoutViewModel model)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //        return RedirectToAction("Login", "Account");

        //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
        //    if (cart == null || !cart.Any())
        //        return RedirectToAction("Index", "Cart");

        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    var order = new Order
        //    {
        //        UserID = userId,
        //        OrderDate = DateTime.Now,
        //        Status = "Pending",
        //        ShippingAddress = model.Address,
        //        TotalAmount = cart.Sum(i => i.Total)
        //    };

        //    _context.Orders.Add(order);
        //    _context.SaveChanges();

        //    foreach (var item in cart)
        //    {
        //        _context.OrdersDetails.Add(new OrderDetail
        //        {
        //            OrderID = order.OrderID,
        //            BookID = item.BookID,
        //            Quantity = item.Quantity,
        //            Price = item.Price
        //        });
        //    }

        //    _context.SaveChanges();

        //    HttpContext.Session.Remove("Cart");

        //    return RedirectToAction("OrderHistory");

        //}

        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
                return RedirectToAction("Index", "Cart");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                ShippingAddress = model.Address,
                TotalAmount = cart.Sum(i => i.Total)
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                _context.OrdersDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }



        // ============================
        // Order History
        // GET: /Orders/OrderHistory
        // ============================
        [HttpGet]
        //public IActionResult OrderHistory()
        //{
        //    int userId = int.Parse(HttpContext.Session.GetString("UserID") ?? "0");

        //    // جلب كل الطلبات الخاصة بالمستخدم
        //    var userOrders = Orders.Where(o => o.UserID == userId).ToList();

        //    return View(userOrders);

        //}

        [HttpGet]
        public IActionResult OrderHistory()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book)
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
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
        public IActionResult Success()
        {
            return View();
        }
        //AcceptOrder
        [HttpPost]
        public IActionResult AcceptOrder(int id)
        {
            // تأكد إن المستخدم أدمن
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            // جلب الأوردر من الـ DB
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            // فقط الأوردرات اللي حالتهم Pending يمكن قبولها
            if (order.Status != "Pending")
            {
                TempData["Message"] = "This order cannot be accepted.";
                return RedirectToAction("Index"); // ممكن توجهيها للـ Admin Orders page
            }

            // تحديث الحالة
            order.Status = "Accepted";
            _context.SaveChanges();

            TempData["Message"] = "Order accepted successfully!";
            return RedirectToAction("Index"); // توجه للصفحة المناسبة
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
