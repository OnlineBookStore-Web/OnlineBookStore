using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using System.Linq;

public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // عرض صفحة الكارت
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        var model = new CartViewModel
        {
            Items = cart,
            CartTotal = cart.Sum(i => i.Price * i.Quantity)
        };
        return View(model);
    }

    // إضافة كتاب للكارت
    [HttpPost]
    public IActionResult AddToCart(int bookID)
    {
        if (!User.Identity.IsAuthenticated)
        {
            TempData["LoginRequired"] = "You must sign in before adding items to your cart.";
            return RedirectToAction("Login", "Account");
        }

        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);
        if (item != null)
        {
            item.Quantity += 1;
        }
        else
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == bookID);
            if (book == null) return NotFound();

            cart.Add(new CartItem
            {
                BookID = book.BookID,
                BookTitle = book.Title,
                Price = book.Price,
                Quantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Details", "Books", new { id = bookID });
    }

    // تحديث كمية عنصر في الكارت
    [HttpPost]
    public IActionResult UpdateQuantity(int bookID, int quantity)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);
        if (item != null)
        {
            item.Quantity = quantity;
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("Index");
    }

    // إزالة عنصر من الكارت
    [HttpPost]
    public IActionResult RemoveItem(int bookID)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        cart.RemoveAll(c => c.BookID == bookID);

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("Index");
    }

    // Checkout - تحويل الكارت إلى Orders في الداتا بيز
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
        if (cart == null || cart.Count == 0)
        {
            TempData["Message"] = "Your cart is empty!";
            return RedirectToAction("Index");
        }

        int userId = int.Parse(HttpContext.Session.GetString("UserID") ?? "0");

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

        // مسح الكارت من Session بعد الدفع
        HttpContext.Session.Remove("Cart");

        return RedirectToAction("OrderHistory", "Orders");
    }
}
