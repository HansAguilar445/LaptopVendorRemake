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
    public class BrandsController : Controller
    {
        private readonly LaptopVendorContext _context;

        public BrandsController(LaptopVendorContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
              return View(await _context.Brands.ToListAsync());
        }

        public IActionResult ViewByBrand()
        {
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        public IActionResult ByBrand()
        {
            return RedirectToAction(nameof(ViewByBrand));
        }

        [HttpPost]
        public IActionResult ByBrand(FilterFormOptions options)
        {
            Brand brand = _context.Brands.Include(brand => brand.Laptops).SingleOrDefault(brand => brand.Id == options.Brand);
            var laptops = brand.Laptops.ToList();

            ViewBag.Brand = brand.Name;

            switch(options.OrderMode)
            {
                case "YearDesc":
                    laptops = laptops.
                        OrderByDescending(laptop => laptop.Year).
                        ToList();
                    ViewBag.OrderMode = "by release year (newest to oldest)";
                    break;
                case "YearAsc":
                    laptops = laptops.
                        OrderBy(laptop => laptop.Year).
                        ToList();
                        ViewBag.OrderMode = "by release year (oldest to newest)";
                    break;
                case "PriceDesc":
                    laptops = laptops.
                        OrderByDescending(laptop => laptop.Price).
                        ToList();
                    ViewBag.OrderMode = "by price (lowest to highest)";
                    break;
                case "PriceAsc":
                    laptops = laptops.
                        OrderBy(laptop => laptop.Price).
                        ToList();
                    ViewBag.OrderMode = "by price (highest to lowest)";
                    break;
            }

            if (options.FilterYear)
            {
                if (options.FilterYearMode == FilterYearMode.Older)
                {
                    laptops = laptops.
                        Where(laptop => laptop.Year < options.Year).
                        ToList();
                    ViewBag.YearFilter = $"Year (older than {options.Year})";
                } else
                {
                    laptops = laptops.
                        Where(laptop => laptop.Year > options.Year).
                        ToList();
                    ViewBag.YearFilter = $"Year (younger than {options.Year})";
                }
            }

            if (options.FilterPrice)
            {
                if (options.FilterPriceMode == FilterPriceMode.Below)
                {
                    laptops = laptops.
                        Where(laptop => laptop.Price < options.Price).
                        ToList();
                    ViewBag.PriceFilter = $"Price (lower than {options.Price})";
                } else
                {
                    laptops = laptops.
                        Where(laptop => laptop.Price > options.Price).
                        ToList();
                    ViewBag.PriceFilter = $"Price (higher than {options.Price})";
                }
            }

            return View(laptops);
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'LaptopVendorContext.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
          return _context.Brands.Any(e => e.Id == id);
        }
    }
}
