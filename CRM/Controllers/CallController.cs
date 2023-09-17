using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRM.Data;
using CRM.Models;
using PagedList;

namespace CRM.Controllers
{
    public class CallController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CallController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Call
        // public async Task<IActionResult> Index()
        // {
        //     var applicationDbContext = _context.Call.Include(c => c.Customer);
        //     return View(await applicationDbContext.ToListAsync());
        // }
        // GET: Call
        // public async Task<IActionResult> Index(int? id)
        // {
        //     if (id == null || _context.Call == null)
        //     {
        //         var applicationDbContext = _context.Call.Include(c => c.Customer);
        //         return View(await applicationDbContext.ToListAsync());
        //     }
        //     else
        //     {
        //         var applicationDbContext = _context.Call.Include(c => c.Customer)
        //             .Where(c => c.Customer.Id == id);
        //         return View(await applicationDbContext.ToListAsync());
        //     }
        // }


        // var calls = await _context.Call
            //     .Include(c => c.Customer)
            //     .Where(m => m.Customer.Id == id);
            // if (calls == null)
            // {
            //     return NotFound();
            // }
            //
            // return View(calls);
        
            public async Task<IActionResult> Index(string sortOrder, string currentFilter, int? pageNumber, int? id)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            
            var calls = _context.Call.Include(c => c.Customer).AsQueryable();

            // If an id is passed, filter the query to specific items
            if (id != null)
            {
                calls = calls.Where(item => item.Customer.Id == id);
            }
            
            
            switch (sortOrder)
            {
                case "name_desc":
                    calls = calls.OrderByDescending(s => s.Customer.Name);
                    break;
                case "Date":
                    calls = calls.OrderBy(s => s.DateOfCall);
                    break;
                case "date_desc":
                    calls = calls.OrderByDescending(s => s.DateOfCall);
                    break;
                default:  // Name ascending 
                    calls = calls.OrderBy(s => s.Customer.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Call>.CreateAsync(calls.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Call/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Call == null)
            {
                return NotFound();
            }

            var call = await _context.Call
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // GET: Call/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customer, "Id", "Id");
            return View();
        }

        // POST: Call/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerID,Subject,Description,DateOfCall,TimeOfCall")] Call call)
        {
            if (ModelState.IsValid)
            {
                _context.Add(call);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "Id", "Id", call.CustomerID);
            return View(call);
        }

        // GET: Call/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Call == null)
            {
                return NotFound();
            }

            var call = await _context.Call.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "Id", "Id", call.CustomerID);
            return View(call);
        }

        // POST: Call/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerID,Subject,Description,DateOfCall,TimeOfCall")] Call call)
        {
            if (id != call.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(call);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.Id))
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
            ViewData["CustomerID"] = new SelectList(_context.Customer, "Id", "Id", call.CustomerID);
            return View(call);
        }

        // GET: Call/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Call == null)
            {
                return NotFound();
            }

            var call = await _context.Call
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: Call/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Call == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Call'  is null.");
            }
            var call = await _context.Call.FindAsync(id);
            if (call != null)
            {
                _context.Call.Remove(call);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallExists(int id)
        {
          return (_context.Call?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
