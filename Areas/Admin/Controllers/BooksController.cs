using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBook.Data;
using IBook.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;
using IBook.Utility;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;

namespace IBook.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SupperAdminEndUser)]
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly HostingEnvironment _hostingEnvironment;
        public BooksController(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index(int page = 1, string findstring = null)
        {
            var applicationDbContext = _context.Books.AsNoTracking().Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).OrderBy(x => x.BookID);
            if(!string.IsNullOrEmpty(findstring))
            {
                applicationDbContext = applicationDbContext.Where(x => x.Title.Contains(findstring)).OrderBy(s => s.BookID);
            }
            var bookList = await PagingList<Book>.CreateAsync(applicationDbContext, 6, page);
            return View(bookList);
        }

        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Admin/Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name");
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["PublisherID"] = new SelectList(_context.Publishers, "ID", "Name");
            return View();
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Summary,Quantities,CreateDate,ModifiedDate,NumberOfPages,Price,ImageUrl,IsPurchase,AuthorID,CategoryID,PublisherID")] Book book)
        {
            if (ModelState.IsValid)
            {
                //Image being saved

                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count != 0)
                {
                    //Image has been uploaded
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uploads, book.Title + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    book.ImageUrl = @"\" + SD.ImageFolder + @"\" + book.Title + extension;
                }
                else
                {
                    //when user does not upload image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultBookImage);
                    System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + book.Title + ".png");
                    book.ImageUrl = @"\" + SD.ImageFolder + @"\" + book.Title + ".png";
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Summary,Quantities,CreateDate,ModifiedDate,NumberOfPages,Price,ImageUrl,IsPurchase,AuthorID,CategoryID,PublisherID")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string webRootPath = _hostingEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count != 0)
                    {
                        //Image has been uploaded
                        var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);

                        using (var filestream = new FileStream(Path.Combine(uploads, book.Title + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        book.ImageUrl = @"\" + SD.ImageFolder + @"\" + book.Title + extension;
                    }
                    else
                    {
                        //when user does not upload image
                        var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultBookImage);
                        System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + book.Title + ".png");
                        book.ImageUrl = @"\" + SD.ImageFolder + @"\" + book.Title + ".png";
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", book.AuthorID);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", book.CategoryID);
            ViewData["PublisherID"] = new SelectList(_context.Publishers, "ID", "Name", book.PublisherID);
            return View(book);
        }

        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b=>b.Publisher)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
