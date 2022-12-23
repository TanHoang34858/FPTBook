using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBook.Data;
using IBook.Models;
using Microsoft.AspNetCore.Authorization;
using IBook.Utility;

namespace IBook.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class OrderBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderBooks
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderBooks.ToListAsync());
        }

        // GET: Admin/OrderBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderBook = await _context.OrderBooks.FindAsync(id);
            if (orderBook == null)
            {
                return NotFound();
            }
            return View(orderBook);
        }

        // POST: Admin/OrderBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate,CustomerName,CustomerAddress,CustomerPhone,isConfirmed")] OrderBook orderBook)
        {
            if (id != orderBook.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderBookExists(orderBook.OrderID))
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
            return View(orderBook);
        }

        // GET: Admin/OrderBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderBook = await _context.OrderBooks
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderBook == null)
            {
                return NotFound();
            }

            return View(orderBook);
        }

        // POST: Admin/OrderBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderBook = await _context.OrderBooks.FindAsync(id);
            _context.OrderBooks.Remove(orderBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderBookExists(int id)
        {
            return _context.OrderBooks.Any(e => e.OrderID == id);
        }
    }
}
