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
    public class ProductManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductManager
        [Route("[area]/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.District).Include(p => p.Seller);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/ProductManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.District)
                .Include(p => p.Seller)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Admin/ProductManager/Create
        [Route("Admin/ProductManager/Create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Domain");
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Email");
            return View();
        }

        // POST: Admin/ProductManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SellerId,Price,Description,Unit,IsPublished,ItemInStock,DistrictId,CategoryId,ImagePath,OfferPrice,OfferExpireDate,CreatedAt,UpdatedAt")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Domain", product.CategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", product.DistrictId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Email", product.SellerId);
            return View(product);
        }


        [HttpGet]
        [Route("Admin/ProductManager/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Domain", product.CategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", product.DistrictId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Email", product.SellerId);
            return View(product);
        }


        [HttpPost]
        public IActionResult Edit(int? id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Domain", product.CategoryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", product.DistrictId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "Id", "Email", product.SellerId);
            return View(product);
        }



        [Route("Admin/ProductManager/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var product = _context.Products.SingleOrDefault(m => m.Id == id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
