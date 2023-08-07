using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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

    
        public IActionResult login()
        {
            return View();
        }

        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string na, string pa)
        {
            SqlConnection conn1 = new SqlConnection("Data Source=ITK6;Initial Catalog=appdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            string sql;
            sql = "SELECT * FROM usersaccounts where name ='" + na + "' and  pass ='" + pa + "' ";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {
                string role = (string)reader["role"];
                string id = Convert.ToString((int)reader["Id"]);

                HttpContext.Session.SetString("Name", na);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("userid", id);
                reader.Close();
                conn1.Close();
                if (role == "customer")
                    return RedirectToAction("catalogue", "books");

                else
                    return RedirectToAction("Index", "books");

            }
            else
            {
                ViewData["Message"] = "wrong user name password";
                return View();
            }
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
        public async Task<IActionResult> Create([Bind("Id,name,pass,email")] useraccounts useraccounts)
        {
            useraccounts.role = "customer";
                _context.Add(useraccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(login));
            
          
        }

        // GET: useraccounts/Edit/5
        public async Task<IActionResult> Edit()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));

            var useraccounts = await _context.useraccounts.FindAsync(id);
         
            return View(useraccounts);
        }

        // POST: useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,pass,role,email")] useraccounts useraccounts)
        {
          
                    _context.Update(useraccounts);
                    await _context.SaveChangesAsync();
          
             return RedirectToAction(nameof(login));
           
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
