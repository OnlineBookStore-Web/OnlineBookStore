// Controllers/CartController.cs
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;

public class CartController : Controller
{
    // Simulate a cart (replace with DB in real project)
    private static List<CartItem> cart = new List<CartItem>();

    public IActionResult Index()
    {
        var model = new CartViewModel { Items = cart };
        return View(model);
    }

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

    [HttpPost]
    public IActionResult RemoveItem(int bookID)
    {
        cart.RemoveAll(c => c.BookID == bookID);
        return RedirectToAction("Index");
    }
}
