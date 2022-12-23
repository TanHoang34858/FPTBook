using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBook.Data;
using IBook.Models;
using IBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBook.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SupperAdminEndUser)]
    [Area("Admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminUsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ApplicationUser.ToList());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }
            var userFromDb = await _context.ApplicationUser.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = _context.ApplicationUser.Where(b => b.Id == id).FirstOrDefault();
                userFromDb.Name = applicationUser.Name;
                userFromDb.PhoneNumber = applicationUser.PhoneNumber;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }
            var userFromDb = await _context.ApplicationUser.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(string id, ApplicationUser applicationUser)
        {
            ApplicationUser userFromDb = _context.ApplicationUser.Where(b => b.Id == id).FirstOrDefault();
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}