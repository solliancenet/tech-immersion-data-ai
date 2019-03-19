using ContosoAutoOpsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ContosoAutoOpsWeb.Controllers
{
    public class ReadOnlyController : Controller
    {
        private readonly Data.ContosoAutoDbReadOnlyContext _context;

        public ReadOnlyController(Data.ContosoAutoDbReadOnlyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var query = new RawSqlString("SELECT DATABASEPROPERTYEX(DB_NAME(), 'Updateability') AS Message");
            var message = _context.Updateability.FromSql(query);

            ViewData["Message"] = message.First().Message;

            var productSalesByStore = (from h in _context.SalesOrderHeaders
                                       join p in _context.SalesPeople on h.SalesPersonID equals p.BusinessEntityID
                                       join s in _context.Stores on p.BusinessEntityID equals s.SalesPersonID
                                       join o in _context.SalesOrderDetails on h.SalesOrderID equals o.SalesOrderID
                                       join pd in _context.Products on o.ProductID equals pd.ProductID
                                       group h by new { Store = s.Name, Product = pd.Name } into g
                                       select new ProductSalesByStore
                                       {
                                           Store = g.Key.Store,
                                           Product = g.Key.Product,
                                           SalesTotal = g.Sum(h => h.TotalDue)
                                       });

            var products = _context.Products;
            return View(productSalesByStore.Take(200));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
