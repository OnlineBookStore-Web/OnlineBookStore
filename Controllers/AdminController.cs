using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

public class AdminController : Controller
{

    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    // Dashboard
    public IActionResult Index()
    {
        ViewBag.TotalBooks = _db.Books.Count();
        ViewBag.TotalUsers = _db.Users.Count();
        ViewBag.TotalOrders = _db.Orders.Count();
        return View();
    }

    // Manage Books
    public IActionResult Books()
    {
        var books = _db.Books.ToList();
        return View(books);
    }

    // Manage Users
    public IActionResult Users()
    {
        var users = _db.Users.ToList();
        return View(users);
    }

    //Manage Orders
    //public IActionResult Orders()
    //{
    //    var orders = _db.Orders
    //        .Include(od => od.Books)
    //        .Include(od => od.Order)
    //            .ThenInclude(o => o.User)
    //        .ToList();

    //    return View(orders);
    //}


    public IActionResult Orders()
    {
        var orderDetails = _db.OrdersDetails
            .Include(od => od.Book)
            .Include(od => od.Order)
                .ThenInclude(o => o.User)
            .ToList();

        return View(orderDetails);
    }
    //GET: Admin/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = _db.Books.FirstOrDefault(b => b.BookID == id);
        if (book == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "Name", book.CategoryId);

        return View(book);
    }

    // POST: Admin/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Book book)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        _db.Books.Update(book);
        _db.SaveChanges();

        return RedirectToAction(nameof(Books));
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(
            _db.Categories,
            "CategoryId",
            "Name"
        );
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Book book)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(
                _db.Categories,
                "CategoryId",
                "Name",
                book.CategoryId
            );
            return View(book);
        }

        _db.Books.Add(book);
        _db.SaveChanges();

        return RedirectToAction(nameof(Books)); 
    }

    //delete
    // GET: Admin/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = _db.Books.FirstOrDefault(b => b.BookID == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book); 
    }
    // POST: Admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        _db.Books.Remove(book);
        _db.SaveChanges();

        return RedirectToAction(nameof(Books)); 
    }

}

