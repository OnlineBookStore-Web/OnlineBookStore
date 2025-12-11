<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Identity;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Security.Claims;
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    // Register 
=======
    // REGISTER
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
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

<<<<<<< HEAD
        return RedirectToAction("Login", "Account");
    }

    // Login 
=======
        return RedirectToAction("Login");
    }


    // LOGIN
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
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
<<<<<<< HEAD
                HttpContext.Session.SetInt32("UserID", user.UserID);
=======
                // Create Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName ?? user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign In
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View();
    }

<<<<<<< HEAD
=======
    // PROFILE
    [HttpGet]
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
    public IActionResult Profile()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        return View(user);
    }

<<<<<<< HEAD
=======
    // EDIT PROFILE 
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
    [HttpGet]
    public IActionResult EditProfile()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        if (user == null) return NotFound();

        user.Password = ""; 
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(User model)
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login");


        if (string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.Remove("Password");
        }

        if (!ModelState.IsValid)
            return View(model);

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.Find(userId);

        if (user == null) return NotFound();

        // Check email uniqueness
        if (_context.Users.Any(u => u.Email == model.Email && u.UserID != userId))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return View(model);
        }

        // Update allowed fields only
        user.FullName = model.FullName;
        user.Email = model.Email;

<<<<<<< HEAD
        if (!string.IsNullOrEmpty(model.Password))
=======
        if (!string.IsNullOrWhiteSpace(model.Password))
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
        {
            user.Password = _passwordHasher.HashPassword(user, model.Password);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Profile");
    }

<<<<<<< HEAD
    public IActionResult Logout()
=======

    // LOGOUT
    public async Task<IActionResult> Logout()
>>>>>>> 8c09a83bfc2f699442de45865ef170ee21acd54d
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}
