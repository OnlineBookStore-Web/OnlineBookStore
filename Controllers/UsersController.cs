using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;

namespace OnlineBookStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var allusers = _context.Users.ToList();
            return View(allusers);
        }
    }
}
