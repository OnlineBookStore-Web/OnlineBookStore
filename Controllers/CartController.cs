// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using OnlineBookStore.Data; // For AppDbContext
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly AppDbContext _context;

    // Simulate a cart (replace with DB in real project)
    private static List<CartItem> cart = new List<CartItem>();

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // Show cart page
    public IActionResult Index()
    {
        var model = new CartViewModel { Items = cart };
        return View(model);
    }

    // Add a book to the cart
    [HttpPost]
    public IActionResult AddToCart(int bookID)
    {
        // Prevent access if user is not logged in
        if (!User.Identity.IsAuthenticated)
        {
            TempData["LoginRequired"] = "You must sign in before adding items to your cart.";
            return RedirectToAction("Login", "Account");
        }

        // Check if the item already exists in the cart
        var item = cart.FirstOrDefault(c => c.BookID == bookID);

        if (item != null)
        {
            item.Quantity += 1;
        }
        else
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == bookID);
            if (book == null)
                return NotFound();

            cart.Add(new CartItem
            {
                BookID = book.BookID,
                BookTitle = book.Title,
                Price = book.Price,
                Quantity = 1
            });
        }

        // Redirect back to the book details page after adding
        return RedirectToAction("Details", "Books", new { id = bookID });
    }


    // Update quantity of a cart item
    [HttpPost]
    public IActionResult UpdateQuantity(int bookID, int quantity)
    {
        var item = cart.FirstOrDefault(c => c.BookID == bookID);
        if (item != null)
        {
            item.Quantity = quantity;
        }
        return RedirectToAction("Index");
    }

    // Remove an item from the cart
    [HttpPost]
    public IActionResult RemoveItem(int bookID)
    {
        cart.RemoveAll(c => c.BookID == bookID);
        return RedirectToAction("Index");
    }
}
