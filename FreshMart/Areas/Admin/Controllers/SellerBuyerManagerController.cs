using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;
using Microsoft.AspNetCore.Authorization;
using FreshMart.Database;
using FreshMart.Core;

namespace FreshMart.Areas.Admin.Controllers
{

    [Area("Admin")]
    //    [Authorize]
    public class SellerBuyerManagerController : Controller
    {
        private readonly AppDbContext _context;

        public SellerBuyerManagerController(AppDbContext context)
        {
            _context = context;
        }




        //SELLER SECTION


        // GET: Admin/SellerBuyerManager
        [Route("Admin/SellerBuyerManager/SellerIndex")]
        public async Task<IActionResult> SellerIndex()
        {

            var applicationDbContext = _context.Sellers.Include(x=>x.User).ThenInclude(x=>x.District);
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
        [HttpGet("Admin/SellerBuyerManager/RequestIndex")]
        public async Task<IActionResult> RequestIndex()
        {
            var sellerRequests = await _context.SellerRequests.Include(s => s.District).AsNoTracking().ToListAsync();
            ViewBag.sellerRequests = sellerRequests;
            return View();
        }


        [HttpGet("Admin/SellerBuyerManager/ApproveSeller/{email}")]
        public async Task<IActionResult> ApproveSeller(string email)
        {
            var requestedSeller = _context.SellerRequests
                .Where(s => s.Email.Contains(email))
                .FirstOrDefault();
            if (requestedSeller == null)
            {
                return NotFound();
            }

            var sellers = _context.Sellers.Include(x => x.User).ThenInclude(x => x.District);
            if (!sellers.Any(x => x.Email == email))
            {
                var seller = new Seller
                {
                    Id = NumberUtilities.GetUniqueNumber(),
                    Name = requestedSeller.SellerName,
                    Email = email,
                    CompanyName = requestedSeller.CompanyName,
                    Phone = requestedSeller.Phone,
                    DateOfBirth = requestedSeller.DateOfBirth,
                    Approval = true
                }; await _context.Sellers.AddAsync(seller);

                var appUserr = _context.Users.Where(x => x.Email == email).FirstOrDefault();
                if (appUserr != null)
                {
                    seller.UserId = appUserr.Id;
                }
                await _context.SaveChangesAsync();
            }

            //REMOVE PREVIOUS SELLER REQUEST
            var del = _context.SellerRequests.Find(requestedSeller.Id);
            _context.SellerRequests.Remove(del);
            await _context.SaveChangesAsync();

            return RedirectToAction("RequestIndex");
        }        //REQUEST SECTION END



        //CUSTOMER SECTION

        // GET: Admin/SellerBuyerManager
        [Route("Admin/SellerBuyerManager/CustomerIndex")]
        public async Task<IActionResult> CustomerIndex()
        {

            var customers = await _context.Customers
                .Include(s => s.User).ThenInclude(x => x.District)
                .AsNoTracking()
                .ToListAsync();
            return View(customers);
        }
        //CUSTOMER SECTION END




        // GET: Admin/SellerBuyerManager/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.User).ThenInclude(x => x.District).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }









        //// GET: Admin/SellerBuyerManager/Create
        //public IActionResult Create()
        //{
        //    ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
        //    return View();
        //}

        //// POST: Admin/SellerBuyerManager/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,DateOfBirth,DistrictId")] Seller seller)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(seller);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(SellerIndex));
        //    }
        //    ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
        //    return View(seller);
        //}









        //// GET: Admin/SellerBuyerManager/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
        //    if (seller == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
        //    return View(seller);
        //}








        //// POST: Admin/SellerBuyerManager/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Email,Phone,DateOfBirth,DistrictId")] Seller seller)
        //{
        //    if (id != seller.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(seller);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SellerExists(seller.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(SellerIndex));
        //    }
        //    ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", seller.DistrictId);
        //    return View(seller);
        //}










        // GET: Admin/SellerBuyerManager/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.User).ThenInclude(x => x.District)
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SellerIndex));
        }

        private bool SellerExists(long id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
