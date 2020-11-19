using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Data;
using FreshMart.Models;
using Microsoft.AspNetCore.Authorization;

namespace FreshMart.Areas.Admin.Controllers
{

    [Area("Admin")]
    //    [Authorize]
    public class SellerBuyerManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellerBuyerManagerController(ApplicationDbContext context)
        {
            _context = context;
        }




        //SELLER SECTION


        // GET: Admin/SellerBuyerManager
        [Route("Admin/SellerBuyerManager/SellerIndex")]
        public async Task<IActionResult> SellerIndex()
        {

            var applicationDbContext = _context.Sellers.Include(s => s.District);
            return View(await applicationDbContext.ToListAsync());
        }


        //post request
        [Route("Admin/SellerBuyerManager/DeleteSeller/{id}")]
        public IActionResult DeleteSeller(string id)
        {
            var r = _context.Sellers.Find(id);
            if (r == null)
            {
                return NotFound();
            }

            _context.Sellers.Remove(r);
            _context.SaveChanges();


            return RedirectToAction("");
        }

        //SELLER SECTION END



        //REQUEST SECTION

        // GET: Admin/SellerBuyerManager
        [Route("Admin/SellerBuyerManager/RequestIndex")]
        public async Task<IActionResult> RequestIndex()
        {

            var applicationDbContext = _context.SellerRequests.Include(s => s.District);
            ViewBag.seller = _context.Sellers.ToList();

            return View(await applicationDbContext.ToListAsync());
        }

        //post request
        [Route("Admin/SellerBuyerManager/approveseller/{email}")]
        public IActionResult ApproveSeller(string email)
        {
            var r = _context.SellerRequests.Where(s => s.Email.Contains(email));
            if (r.SingleOrDefault() == null)
            {
                return NotFound();
            }

            var request = _context.SellerRequests.Where(s => s.Email.Contains(email)).SingleOrDefault();

            var seller = new Seller
            {
                Name = request.SellerName,
                Email = email,
                CompanyName = request.CompanyName,
                DistrictId = request.DistrictId,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Approval = true
            };
            _context.Sellers.Add(seller);
            _context.SaveChanges();



            var del = _context.SellerRequests.Find(request.Id);
            _context.SellerRequests.Remove(del);
            _context.SaveChanges();

            var applicationDbContext = _context.SellerRequests.Include(s => s.District);
            ViewBag.seller = _context.Sellers.ToList();

            return View("RequestIndex", applicationDbContext);
        }


        //REQUEST SECTION END



        //CUSTOMER SECTION


        // GET: Admin/SellerBuyerManager
        [Route("Admin/SellerBuyerManager/CustomerIndex")]
        public async Task<IActionResult> CustomerIndex()
        {

            var applicationDbContext = _context.Customers.Include(s => s.District);
            return View(await applicationDbContext.ToListAsync());
        }




        //CUSTOMER SECTION END




        // GET: Admin/SellerBuyerManager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }









        // GET: Admin/SellerBuyerManager/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
            return View();
        }

        // POST: Admin/SellerBuyerManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,DateOfBirth,DistrictId")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seller);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SellerIndex));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
            return View(seller);
        }









        // GET: Admin/SellerBuyerManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
            return View(seller);
        }








        // POST: Admin/SellerBuyerManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,DateOfBirth,DistrictId")] Seller seller)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SellerIndex));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
            return View(seller);
        }










        // GET: Admin/SellerBuyerManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }









        // POST: Admin/SellerBuyerManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SellerIndex));
        }

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
