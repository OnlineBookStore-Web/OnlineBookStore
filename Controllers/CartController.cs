using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
