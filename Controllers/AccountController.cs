using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    //  Register 
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User model)
    {
        if (!ModelState.IsValid) return View(model);

        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return View(model);
        }

        model.Password = _passwordHasher.HashPassword(model, model.Password);

        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login", "Account"); 
    }

    //  Login 
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetInt32("UserID", user.UserID);
                return RedirectToAction("Index", "Home"); 
            }
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View();
    }

    //  Profile 
    public IActionResult Profile()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null) return RedirectToAction("Login");

        var user = _context.Users.Find(userId.Value);
        return View(user);
    }

    // Edit Profile 
    [HttpGet]
    public IActionResult EditProfile(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.Password = string.Empty;
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(User model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _context.Users.FindAsync(model.UserID);
        if (user == null) return NotFound();

        if (_context.Users.Any(u => u.Email == model.Email && u.UserID != model.UserID))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return View(model);
        }

        user.FullName = model.FullName;
        user.Email = model.Email;


        if (!string.IsNullOrEmpty(model.Password))
        {
            if (model.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters");
                return View(model);
            }

            user.Password = _passwordHasher.HashPassword(user, model.Password);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Profile");
    }

    //Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
