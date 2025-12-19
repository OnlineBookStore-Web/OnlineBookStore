using Microsoft.AspNetCore.Mvc;
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

    // Manage Orders
    //public IActionResult Orders()
    //{
    //    var orders = _db.Orders.ToList();
    //.Include(od => od.Book)
    //.Include(od => od.Order)
    //    .ThenInclude(o => o.User)
    //.ToList();

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

}
