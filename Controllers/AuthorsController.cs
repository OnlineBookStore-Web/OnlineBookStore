using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        // READ - Index
        public IActionResult Index()
        {
            var allAuthors = _context.Authors.ToList();
            return View(allAuthors);
        }

        // READ - Details
        public async Task<IActionResult> Details(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorID == id);

            if (author == null)
                return NotFound();

            return View(author);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Authors author)
        {
            if (!ModelState.IsValid)
                return View(author);

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT - GET
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Authors author)
        {
            if (id != author.AuthorID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(author);

            _context.Authors.Update(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
