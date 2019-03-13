using ContosoAutoOpsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace ContosoAutoOpsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly Data.ContosoAutoDbContext _context;

        public HomeController(Data.ContosoAutoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products;
            return View(products.Take(25));
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.First(p => p.ProductID == id);
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
