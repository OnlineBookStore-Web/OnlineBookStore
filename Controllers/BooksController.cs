using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Linq;

namespace OnlineBookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // GET: Books/Details/5
        public IActionResult Details(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);

            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
