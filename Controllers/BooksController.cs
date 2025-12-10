using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // =========================
        //        READ (Index)
        // =========================
        public IActionResult Index(string category, string search)
        {
            var books = _context.Books
                .Include(b => b.Author) // FIX #1 — load author data
                .AsQueryable();

            // Category filter (case-insensitive)
            if (!string.IsNullOrEmpty(category))
            {
                books = books.Where(b => b.Category.ToLower() == category.ToLower());
            }

            // Search filter (case-insensitive)
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                books = books.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Author.Name.ToLower().Contains(search));
            }

            return View(books.ToList());
        }


        // =========================
        //        DETAILS
        // =========================
        public IActionResult Details(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // =========================
        //        CREATE
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            _context.Books.Add(book);   // <<< fixed
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        //        EDIT
        // =========================
        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.BookID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(book);

            _context.Update(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        //        DELETE
        // =========================
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
