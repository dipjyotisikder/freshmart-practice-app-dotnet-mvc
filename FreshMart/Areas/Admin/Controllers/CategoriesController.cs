using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Data;
using FreshMart.Models;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        [Route("Admin/Categories")]
        public IActionResult Index()
        {
            var cats = _context.Categories.ToList();

            return View(cats);
        }



        // GET: Admin/Categories/Create
        [Route("Admin/Categories/Create")]
        public IActionResult Create()
        {
            var cats = _context.CategoryDomains.ToList();
            var vm = new CategoryViewModel
            {
                CategoryDomains = cats
            };
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Categories/Create")]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var vm = new Category
                {
                    Name = viewModel.Category.Name,
                    Domain = viewModel.Category.Domain

                };

                _context.Categories.Add(vm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Admin/Categories/Edit/5
        [HttpGet]
        [Route("Admin/Categories/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.SingleOrDefaultAsync(m => m.Id == id);
            var domain = await _context.CategoryDomains.SingleOrDefaultAsync(m => m.Name == category.Domain);
            var domains = await _context.CategoryDomains.ToListAsync();

            if (category == null)
            {
                return NotFound();
            }

            var vm = new CategoryViewModel
            {
                Category = category,
                CategoryDomain = domain,
                CategoryDomains = domains
            };

            return View(vm);
        }


        [HttpPost]
        //        [Route("Admin/Categories/Edit")]
        public IActionResult Edit(int id, CategoryViewModel categoryView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryView.Category);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryView.Category.Id))
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
            return View();
        }




        // POST: Admin/Categories/Delete/5

        [Route("Admin/Categories/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var category = _context.Categories.SingleOrDefault(m => m.Id == id);

            var products = _context.Products.Where(p => p.CategoryId == id).ToList();


            //delete products first
            foreach (var item in products)
            {
                _context.Products.Remove(item);
                _context.SaveChanges();
            }

            //delete category then
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
