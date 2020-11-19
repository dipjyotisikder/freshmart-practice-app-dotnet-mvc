using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;
using FreshMart.Database;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryDomainsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryDomainsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryDomains
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryDomains.ToListAsync());
        }



        // GET: Admin/CategoryDomains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryDomains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CategoryDomain categoryDomain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryDomain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDomain);
        }

        // GET: Admin/CategoryDomains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryDomain = await _context.CategoryDomains.SingleOrDefaultAsync(m => m.Id == id);
            if (categoryDomain == null)
            {
                return NotFound();
            }
            return View(categoryDomain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CategoryDomain categoryDomain)
        {
            if (id != categoryDomain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryDomain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryDomainExists(categoryDomain.Id))
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
            return View(categoryDomain);
        }



        // POST: Admin/CategoryDomains/Delete/5

        [Route("Admin/CategoryDomains/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var categoryDomain = _context.CategoryDomains.SingleOrDefault(m => m.Id == id);

            var categories = _context.Categories.Where(c => c.Domain == categoryDomain.Name).ToList();

            foreach (var cat in categories)
            {

                _context.Categories.Remove(cat);
                _context.SaveChangesAsync();
            }

            _context.CategoryDomains.Remove(categoryDomain);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool CategoryDomainExists(int id)
        {
            return _context.CategoryDomains.Any(e => e.Id == id);
        }
    }
}
