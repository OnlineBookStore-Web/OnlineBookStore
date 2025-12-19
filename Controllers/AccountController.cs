using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    // ============================
    // REGISTER
    // ============================
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return View(model);
        }

        model.Password = _passwordHasher.HashPassword(model, model.Password);
        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }

    // ============================
    // LOGIN
    // ============================
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

                // Sign In
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View();
    }

    // ============================
    // PROFILE
    // ============================
    [HttpGet]
    public IActionResult Profile()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        return View(user);
    }

    // ============================
    // EDIT PROFILE
    // ============================
    [HttpGet]
    public IActionResult EditProfile()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        if (user == null)
            return NotFound();

        user.Password = "";
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(User model)
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");

        if (string.IsNullOrWhiteSpace(model.Password))
            ModelState.Remove("Password");

        if (!ModelState.IsValid)
            return View(model);

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        if (user == null)
            return NotFound();

        // Check email uniqueness
        if (_context.Users.Any(u => u.Email == model.Email && u.UserID != userId))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return View(model);
        }

        user.FullName = model.FullName;
        user.Email = model.Email;

        if (!string.IsNullOrWhiteSpace(model.Password))
            user.Password = _passwordHasher.HashPassword(user, model.Password);

        await _context.SaveChangesAsync();

        return RedirectToAction("Profile");
    }

    // ============================
    // LOGOUT
    // ============================
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }



}
