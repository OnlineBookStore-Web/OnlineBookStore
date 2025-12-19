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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var order = _context.Orders
                .FirstOrDefault(o => o.OrderID == id && o.UserID == userId);

            if (order == null)
                return NotFound();

            if (order.Status == "Pending")
            {
                order.Status = "Cancelled";
                _context.SaveChanges();
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
