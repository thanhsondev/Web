using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Data;

namespace Web.Models
{
    public class useraccountsController : Controller
    {
        private readonly WebContext _context;

        public useraccountsController(WebContext context)
        {
            _context = context;
        }

        // GET: useraccounts
        public async Task<IActionResult> Index()
        {
              return _context.useraccounts != null ? 
                          View(await _context.useraccounts.ToListAsync()) :
                          Problem("Entity set 'WebContext.useraccounts'  is null.");
        }

        // GET: useraccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.useraccounts == null)
            {
                return NotFound();
            }

            var useraccounts = await _context.useraccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccounts == null)
            {
                return NotFound();
            }

            return View(useraccounts);
        }

        // GET: useraccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,pass,role,email")] useraccounts useraccounts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(useraccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(useraccounts);
        }

        // GET: useraccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.useraccounts == null)
            {
                return NotFound();
            }

            var useraccounts = await _context.useraccounts.FindAsync(id);
            if (useraccounts == null)
            {
                return NotFound();
            }
            return View(useraccounts);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,pass,role,email")] useraccounts useraccounts)
        {
            if (id != useraccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(useraccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!useraccountsExists(useraccounts.Id))
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
            return View(useraccounts);
        }

        // GET: useraccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.useraccounts == null)
            {
                return NotFound();
            }

            var useraccounts = await _context.useraccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccounts == null)
            {
                return NotFound();
            }

            return View(useraccounts);
        }

        // POST: useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.useraccounts == null)
            {
                return Problem("Entity set 'WebContext.useraccounts'  is null.");
            }
            var useraccounts = await _context.useraccounts.FindAsync(id);
            if (useraccounts != null)
            {
                _context.useraccounts.Remove(useraccounts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool useraccountsExists(int id)
        {
          return (_context.useraccounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
