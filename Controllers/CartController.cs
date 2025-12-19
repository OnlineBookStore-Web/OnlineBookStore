using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using System.Linq;

public class CartController : Controller
{
    private readonly AppDbContext _context;

<<<<<<< HEAD
    // Simulate a cart (replace with DB in real project)
    //private static List<CartItem> cart = new List<CartItem>();
    public static List<CartItem> Cart { get; } = new List<CartItem>();

=======
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // عرض صفحة الكارت
    public IActionResult Index()
    {
<<<<<<< HEAD
        var model = new CartViewModel { Items = Cart };
=======
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        var model = new CartViewModel
        {
            Items = cart,
            CartTotal = cart.Sum(i => i.Price * i.Quantity)
        };
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
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

<<<<<<< HEAD
        // Check if the item already exists in the cart
        var item = Cart.FirstOrDefault(c => c.BookID == bookID);
=======
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b

        var item = cart.FirstOrDefault(c => c.BookID == bookID);
        if (item != null)
        {
            item.Quantity += 1;
        }
        else
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == bookID);
            if (book == null) return NotFound();

            Cart.Add(new CartItem
            {
                BookID = book.BookID,
                BookTitle = book.Title,
                Price = book.Price,
                Quantity = 1
            });
        }

<<<<<<< HEAD
        // Redirect back to the book details page after adding
        //return RedirectToAction("Details", "Books", new { id = bookID });
        return Redirect(Request.Headers["Referer"].ToString());

=======
        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Details", "Books", new { id = bookID });
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
    }

    // تحديث كمية عنصر في الكارت
    [HttpPost]
    public IActionResult UpdateQuantity(int bookID, int quantity)
    {
<<<<<<< HEAD
        var item = Cart.FirstOrDefault(c => c.BookID == bookID);
=======
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
        if (item != null)
        {
            item.Quantity = quantity;
        }
<<<<<<< HEAD
        //return RedirectToAction("Index");
        return Redirect(Request.Headers["Referer"].ToString());

=======

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("Index");
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
    }

    // إزالة عنصر من الكارت
    [HttpPost]
    public IActionResult RemoveItem(int bookID)
    {
<<<<<<< HEAD
        Cart.RemoveAll(c => c.BookID == bookID);
        //return RedirectToAction("Index");
        return Redirect(Request.Headers["Referer"].ToString());

=======
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        cart.RemoveAll(c => c.BookID == bookID);

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("Index");
>>>>>>> d5bc6e1a68c56f22eac0c1dad56e7083bac0434b
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
