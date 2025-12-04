using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbcontext _context;
        public BooksController(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}

