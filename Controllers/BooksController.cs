using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // =============================
        //           INDEX
        // =============================
        public IActionResult Index(string search, int? category, string sort)
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
                books = books.Where(b => b.Title.Contains(search) || b.Author.Name.Contains(search));

            // Filter
            if (category.HasValue)
                books = books.Where(b => b.CategoryId == category.Value);

            // Sorting
            switch (sort)
            {
                case "price_asc":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                case "popularity":
                    books = books.OrderByDescending(b => b.Sales);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            ViewBag.Categories = _context.Categories.ToList(); // ⭐ send categories to view
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedCategoryName = category.HasValue
            ? _context.Categories.FirstOrDefault(c => c.CategoryId == category.Value)?.Name
            : "All Categories";

            return View(books.ToList());
        }


        // =============================
        //         DETAILS
        // =============================
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .Include(b => b.Category)   // 🔥 REQUIRED
                .FirstOrDefault(b => b.BookID == id);

            if (book == null)
                return NotFound();

            return View(book);
        }


        // =============================
        //        ADD REVIEW
        // =============================
        [HttpPost]
        public IActionResult AddReview(int bookId, string userName, int rating, string comment)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(comment))
                return RedirectToAction("Details", new { id = bookId });

            var review = new Review
            {
                BookID = bookId,
                UserName = userName,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = bookId });
        }


        // =============================
        //            CREATE
        // =============================
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
                return View(book);
            }


            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =============================
        //             EDIT
        // =============================
        //public IActionResult Edit(int id)
        //{
        //    var book = _context.Books.FirstOrDefault(b => b.BookID == id);
        //    if (book == null)
        //        return NotFound();

        //    return View(book);
        //}

        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookID == id);
            if (book == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", book.CategoryId);
            return View(book);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.BookID)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
                return View(book);
            }

            _context.Update(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, Book updatedBook)
        //{
        //    if (id != updatedBook.BookID)
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.Categories = new SelectList(
        //            _context.Categories.ToList(),
        //            "CategoryId",
        //            "Name",
        //            updatedBook.CategoryId
        //        );
        //        return View(updatedBook);
        //    }

        //    var book = _context.Books.FirstOrDefault(b => b.BookID == id);
        //    if (book == null)
        //        return NotFound();

        //    // ✅ Explicit updates (SAFE)
        //    book.Title = updatedBook.Title;
        //    book.AuthorID = updatedBook.AuthorID;
        //    book.CategoryId = updatedBook.CategoryId;
        //    book.Price = updatedBook.Price;
        //    book.Stock = updatedBook.Stock;
        //    book.Description = updatedBook.Description;   // 🔥 FIX
        //    book.ImageUrl = updatedBook.ImageUrl;

        //    _context.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}


        // =============================
        //             DELETE
        // =============================
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
