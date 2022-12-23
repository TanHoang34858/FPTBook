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
    public class OrderBookDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderBookDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderBookDetails
        public async Task<IActionResult> Index(int id)
        {
            var temp = _context.OrderBookDetails.Where(x => x.OrderBookID == id).Include(o => o.Book);
            return View(temp);
        }
    }
}
