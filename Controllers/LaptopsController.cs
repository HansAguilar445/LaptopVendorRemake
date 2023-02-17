using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopVendorRemake.Data;
using LaptopVendorRemake.Models;

namespace LaptopVendorRemake.Controllers
{
    public class LaptopsController : Controller
    {
        private readonly LaptopVendorContext _context;

        public LaptopsController(LaptopVendorContext context)
        {
            _context = context;
        }

        public IActionResult CheapestLaptops()
        {
            var laptops = _context.Laptops.
                Include(laptop => laptop.Brand).
                OrderBy(laptop => laptop.Price).
                Take(3);
            return View(laptops);
        }

        // GET: Laptops
        public async Task<IActionResult> Index()
        {
            var laptopVendorContext = _context.Laptops.Include(l => l.Brand);
            return View(await laptopVendorContext.ToListAsync());
        }

        // GET: Laptops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Laptops == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .Include(l => l.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Laptops/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Price,Year,BrandId")] Laptop laptop)
        {
            Brand brand = await _context.Brands.SingleOrDefaultAsync(brand => brand.Id == laptop.BrandId);
            laptop.Brand = brand;

            //Would have kept ModelState.IsValid if it didn't return false because laptop.Brand was null when everything else was fine
            //since there is no real way I know at the time of writing this to attach Brand to Laptop through the scaffolded create form
            if (LaptopIsValid(laptop))
            {
                brand.Laptops.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", laptop.BrandId);
            return View(laptop);
        }

        // GET: Laptops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Laptops == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", laptop.BrandId);
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Price,Year,BrandId")] Laptop laptop)
        {
            if (id != laptop.Id)
            {
                return NotFound();
            }

            Brand brand = await _context.Brands.SingleOrDefaultAsync(brand => brand.Id == laptop.BrandId);
            laptop.Brand = brand;

            if (LaptopIsValid(laptop))
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", laptop.BrandId);
            return View(laptop);
        }

        // GET: Laptops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Laptops == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .Include(l => l.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Laptops == null)
            {
                return Problem("Entity set 'LaptopVendorContext.Laptops'  is null.");
            }
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
          return _context.Laptops.Any(e => e.Id == id);
        }

        private bool LaptopIsValid(Laptop laptop)
        {
            if (laptop == null)
            {
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(laptop.Model))
            {
                return false;
            }

            if (laptop.Brand == null)
            {
                return false;
            }

            return true;
        }
    }
}
