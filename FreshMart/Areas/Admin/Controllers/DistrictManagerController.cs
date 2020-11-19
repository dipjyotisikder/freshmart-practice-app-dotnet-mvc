using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Data;
using FreshMart.Models;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DistrictManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistrictManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryDomains
        public async Task<IActionResult> Index()
        {
            return View(await _context.Districts.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,Division,Name")] District district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(district);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        // GET: Admin/CategoryDomains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.Districts.SingleOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Division,Name")] District district)
        {
            if (id != district.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.Id))
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
            return View(district);
        }



        // POST: Admin/CategoryDomains/Delete/5

        [Route("Admin/CategoryDomains/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var district = _context.Districts.SingleOrDefault(m => m.Id == id);



            _context.Districts.Remove(district);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
