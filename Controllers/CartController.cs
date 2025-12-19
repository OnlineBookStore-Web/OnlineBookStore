using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using OnlineBookStore.Helpers;
using System.Linq;

public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // =============================
    // CART PAGE
    // =============================
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        return View(new CartViewModel
        {
            Items = cart,
            CartTotal = cart.Sum(i => i.Price * i.Quantity)
        });
    }

    // =============================
    // ADD TO CART
    // =============================
    [HttpPost]
    public IActionResult AddToCart(int bookID)
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login", "Account");

        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);

        if (item != null)
        {
            item.Quantity++;
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

        return Redirect(Request.Headers["Referer"].ToString());
>>>>>>> 49d68484d0222a007ed4dc1113008d2d760421e6
    }

    // =============================
    // UPDATE QTY
    // =============================
    [HttpPost]
    public IActionResult UpdateQuantity(int bookID, int quantity)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);

        if (item != null)
        {
            if (quantity <= 0)
                cart.Remove(item);
            else
                item.Quantity = quantity;
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return Redirect(Request.Headers["Referer"].ToString());
    }

    // =============================
    // CHECKOUT
    // =============================
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        if (cart == null || !cart.Any())
        {
            TempData["Message"] = "Your cart is empty!";
            return RedirectToAction("Index");
        }

        int userId = int.Parse(HttpContext.Session.GetString("UserID") ?? "0");

        foreach (var item in cart)
        {
            _context.Orders.Add(new Order
            {
                UserID = userId,
                BookID = item.BookID,
                Quantity = item.Quantity,
                OrderDate = DateTime.Now
            });
        }

        _context.SaveChanges();

        // CLEAR CART
        HttpContext.Session.Remove("Cart");

        return RedirectToAction("OrderHistory", "Orders");
    }
}
>>>>>>> 49d68484d0222a007ed4dc1113008d2d760421e6
