using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Helpers;
using OnlineBookStore.Models;
using System.Linq;
using System.Security.Claims;

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
            CartTotal = cart.Sum(i => i.Total)
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
            var book = _context.Books.Find(bookID);
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
    // REMOVE ITEM
    // =============================
    [HttpPost]
    public IActionResult RemoveItem(int bookID)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        var item = cart.FirstOrDefault(c => c.BookID == bookID);
        if (item != null)
        {
            cart.Remove(item);
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);

        // رجوع لصفحة الكارت بعد المسح
        return RedirectToAction("Index");
    }


    // =============================
    // CHECKOUT
    // =============================
    //public IActionResult Checkout()
    //{

    //    if (!User.Identity.IsAuthenticated)
    //        return RedirectToAction("Login", "Account");

    //    int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

    //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

    //    if (cart == null || !cart.Any())
    //    {
    //        TempData["Message"] = "Your cart is empty!";
    //        return RedirectToAction("Index");
    //    }

    //    var order = new Order
    //    {
    //        UserID = userId,
    //        OrderDate = DateTime.Now,
    //        Status = "Pending",
    //        TotalAmount = cart.Sum(i => i.Price * i.Quantity)
    //    };

    //    _context.Orders.Add(order);
    //    try
    //    {
    //        _context.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.InnerException?.Message ?? ex.Message);
    //    }


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

    //    return RedirectToAction("Checkout", "Orders");
    //}

}

