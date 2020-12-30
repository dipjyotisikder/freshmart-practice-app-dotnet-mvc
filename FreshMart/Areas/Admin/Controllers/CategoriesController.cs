using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;
using FreshMart.Database;
using FreshMart.Core;
using FreshMart.ViewModels;
using FreshMart.Core.Utilities;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        [Route("Admin/Categories")]
        public async Task<IActionResult> Index()
        {
            var cats = await _context.Categories.Where(x => !x.IsParent).AsNoTracking().ToListAsync();
            return View(cats);
        }


        // GET: Admin/Categories
        [Route("Admin/Categories/Parents")]
        public async Task<IActionResult> Parents()
        {
            var cats = await _context.Categories.Where(x => x.IsParent).AsNoTracking().ToListAsync();
            return View(cats);
        }



        // GET: Admin/Categories/Create
        [Route("Admin/Categories/Create")]
        public async Task<IActionResult> Create()
        {
            var cats = await _context.Categories.AsNoTracking().ToListAsync();
            var vm = new CategoryViewModel
            {
                Categories = cats
            };
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Categories/Create")]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vm = new Category
                {
                    Id = NumberUtilities.GetUniqueNumber(),
                    Name = model.Category.Name,
                    ParentId = model.Category.ParentId
                };
                await _context.Categories.AddAsync(vm);

                if (model.Category.ParentId != null)
                {
                    var cat = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Category.ParentId);
                    cat.IsParent = true;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Admin/Categories/Edit/5
        [HttpGet]
        [Route("Admin/Categories/Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.Include(x => x.Parent).SingleOrDefaultAsync(m => m.Id == id);
            var all = await _context.Categories.Where(x => x.Id != id).ToListAsync();

            if (category == null)
            {
                return NotFound();
            }

            var vm = new CategoryViewModel
            {
                Category = category,
                Categories = all,
                Parent = category.Parent
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(long id, CategoryViewModel model)
        {
            if (model.Parent?.Id == null)
            {
                return null;
            }
            try
            {
                var category = await _context.Categories.Where(x => x.Id == id).Include(x => x.Parent).FirstOrDefaultAsync();
                //MAKE PREVIOUS PARENT AS NOTHING
                if (category.Parent != null)
                {
                    var isOtherAnyOnesParentToo = await _context.Categories.Where(x => x.ParentId == category.Parent.Id && x.Id != id).AnyAsync();
                    if (!isOtherAnyOnesParentToo)
                    {
                        category.Parent.IsParent = false;
                        await _context.SaveChangesAsync();
                    }

                }

                category.Name = model.Category.Name;

                //MARK CURRENT PARENT AS NEW PARENT
                var parent = await _context.Categories.Where(x => x.Id == model.Parent.Id).Include(x => x.Parent).FirstOrDefaultAsync();
                if (parent != null)
                {
                    category.ParentId = parent.Id;
                    parent.IsParent = true;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return await Task.FromResult(RedirectToAction(nameof(Index)));
        }


        [Route("Admin/Categories/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(long? id)
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

        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
