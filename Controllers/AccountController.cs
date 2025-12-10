using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    // GET: User Entry Page (Sign In or Sign Up)
    public IActionResult UserEntry()
    {
        return View();
    }

    // GET: Admin Login
    public IActionResult AdminLogin()
    {
        return View();
    }

    // POST: Admin Login
    [HttpPost]
    public IActionResult AdminLogin(string email, string password)
    {
        var admin = _db.Users.SingleOrDefault(u => u.Email == email && u.Password == password && u.Role == "Admin");
        if (admin == null)
        {
            ModelState.AddModelError("", "Invalid Admin credentials");
            return View();
        }

        HttpContext.Session.SetString("UserEmail", admin.Email);
        HttpContext.Session.SetString("UserRole", admin.Role);

        return RedirectToAction("Dashboard", "Admin");
    }
}
