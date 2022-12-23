using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBook.Models;
using IBook.Data;
using Microsoft.EntityFrameworkCore;
using IBook.Extensions;
using ReflectionIT.Mvc.Paging;

namespace IBook.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int page = 1, string findstring = null)
        {
            var Query = _db.Books.AsNoTracking().OrderBy(x => x.BookID);
            if(!string.IsNullOrEmpty(findstring))
            {
                Query = Query.Where(x => x.Title.Contains(findstring)).OrderBy(x => x.BookID);
            }
            var bookList = await PagingList<Book>.CreateAsync(Query, 6, page);
            return View(bookList);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b=>b.Publisher)
                .FirstOrDefaultAsync(m => m.BookID == id);
            return View(book);
        }
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart==null)
            {
                lstShoppingCart = new List<int>();
            }
            int flag = 0;
            foreach(int item in lstShoppingCart)
            {
                if (item == id)
                    flag++;
            }
            if(flag == 0)
                lstShoppingCart.Add(id);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction("Index", "Home", new { area = "Customer" }); 
        }
        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart.Count > 0)
            {
                if(lstShoppingCart.Contains(id))
                {
                    lstShoppingCart.Remove(id);
                }
            }
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction(nameof(Index));
        }
    }
}
