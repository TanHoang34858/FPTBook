using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBook.Data;
using IBook.Models;
using IBook.Models.ViewModel;
using IBook.Extensions;

namespace IBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartsController(ApplicationDbContext context)
        {
            _context = context;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                books = new List<Models.Book>()
            };
        }

        // GET: Customer/ShoppingCarts
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                foreach (int cartItem in lstShoppingCart)
                {
                    Book _book = _context.Books.Include(p => p.Author).Include(p => p.Category).Where(p => p.BookID == cartItem).FirstOrDefault();
                    ShoppingCartVM.books.Add(_book);
                }
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItem = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            ShoppingCartVM.orderBook.AppointmentDate = ShoppingCartVM.orderBook.AppointmentDate.AddHours(ShoppingCartVM.orderBook.AppointmentTime.Hour).AddMinutes(ShoppingCartVM.orderBook.AppointmentTime.Minute);

            OrderBook orderBooks = ShoppingCartVM.orderBook;
            _context.OrderBooks.Add(orderBooks);
            _context.SaveChanges();

            int orderbookid = orderBooks.OrderID;

            foreach (int bookid in lstCartItem)
            {
                OrderBookDetail orderBookDetail = new OrderBookDetail()
                {
                    BookID = bookid,
                    OrderBookID = orderbookid
                };
                _context.OrderBookDetails.Add(orderBookDetail);
                var _book = _context.Books.Where(x => x.BookID == bookid).SingleOrDefault();
                if (_book != null)
                {
                    _book.Quantities -= 1;
                    if (_book.Quantities == 0)
                    {
                        _book.IsPurchase = false;
                    }
                }
            }
            _context.SaveChanges();

            lstCartItem = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItem);

            return RedirectToAction("OrderBookConfirmed", "ShoppingCarts", new { Id = orderbookid });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderBookConfirmed(int id)
        {
            ShoppingCartVM.orderBook = _context.OrderBooks.Where(x => x.OrderID == id).FirstOrDefault();
            List<OrderBookDetail> objOrderList = _context.OrderBookDetails.Where(x => x.OrderBookID == id).ToList();

            foreach (OrderBookDetail orderBookDetail in objOrderList)
            {
                ShoppingCartVM.books.Add(_context.Books.Include(x => x.Author).Include(x => x.Category).Where(x => x.BookID == orderBookDetail.BookID).FirstOrDefault());
            }
            return View(ShoppingCartVM);
        }





















































        // GET: Customer/ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderBookDetail = await _context.OrderBookDetails
                .Include(o => o.Book)
                .Include(o => o.OrderBook)
                .FirstOrDefaultAsync(m => m.OrderBookID == id);
            if (orderBookDetail == null)
            {
                return NotFound();
            }

            return View(orderBookDetail);
        }

        // GET: Customer/ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Books, "BookID", "ImageUrl");
            ViewData["OrderBookID"] = new SelectList(_context.OrderBooks, "OrderID", "CustomerAddress");
            return View();
        }

        // POST: Customer/ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderBookID,BookID,Quantities")] OrderBookDetail orderBookDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderBookDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Books, "BookID", "ImageUrl", orderBookDetail.BookID);
            ViewData["OrderBookID"] = new SelectList(_context.OrderBooks, "OrderID", "CustomerAddress", orderBookDetail.OrderBookID);
            return View(orderBookDetail);
        }

        // GET: Customer/ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderBookDetail = await _context.OrderBookDetails.FindAsync(id);
            if (orderBookDetail == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Books, "BookID", "ImageUrl", orderBookDetail.BookID);
            ViewData["OrderBookID"] = new SelectList(_context.OrderBooks, "OrderID", "CustomerAddress", orderBookDetail.OrderBookID);
            return View(orderBookDetail);
        }

        // POST: Customer/ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderBookID,BookID,Quantities")] OrderBookDetail orderBookDetail)
        {
            if (id != orderBookDetail.OrderBookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderBookDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderBookDetailExists(orderBookDetail.OrderBookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Books, "BookID", "ImageUrl", orderBookDetail.BookID);
            ViewData["OrderBookID"] = new SelectList(_context.OrderBooks, "OrderID", "CustomerAddress", orderBookDetail.OrderBookID);
            return View(orderBookDetail);
        }

        // GET: Customer/ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderBookDetail = await _context.OrderBookDetails
                .Include(o => o.Book)
                .Include(o => o.OrderBook)
                .FirstOrDefaultAsync(m => m.OrderBookID == id);
            if (orderBookDetail == null)
            {
                return NotFound();
            }

            return View(orderBookDetail);
        }

        // POST: Customer/ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderBookDetail = await _context.OrderBookDetails.FindAsync(id);
            _context.OrderBookDetails.Remove(orderBookDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderBookDetailExists(int id)
        {
            return _context.OrderBookDetails.Any(e => e.OrderBookID == id);
        }
    }
}
