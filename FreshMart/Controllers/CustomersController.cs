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

namespace FreshMart.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        [Route("Customers")]
        public IActionResult Index()
        {
            var app = _context.Customers.Where(c => c.Email == User.Identity.Name);
            if (app.SingleOrDefault() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var customer = _context.Customers.Where(c => c.Email == User.Identity.Name)
                .Include(x => x.User).AsNoTracking()
                .FirstOrDefault();

            ViewBag.districts = _context.Districts.AsNoTracking().ToList();

            return View(customer);
        }




        // POST: Customers/Edit/5

        [HttpPost]
        [Route("Customers/Edit")]
        public IActionResult Edit(Customer customer)
        {
            if (customer.Id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", customer.User.DistrictId);
            return View(customer);
        }


        [Route("Customers/orders")]
        public IActionResult Orders()
        {
            var customer = _context.Customers.Where(c => c.Email == User.Identity.Name).FirstOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = _context.Orders.Where(c => c.CustomerId == customer.Id).AsNoTracking().ToList();

            var agentOrders = from ao in _context.AgentOrders.AsNoTracking()
                              join o in orders
                              on ao.OrderId equals o.Id
                              select ao;

            ViewBag.orders = orders;
            ViewBag.agentOrders = agentOrders;
            ViewBag.districts = _context.Districts.AsNoTracking().ToList();

            return View(orders);
        }



        [Route("Admin/Customers/DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var orderchk = _context.Orders.Include(c => c.Customer).Where(c => c.Id == id).FirstOrDefault();

            if (User.Identity.Name != orderchk.Customer.Email)
            {
                return NotFound();
            }

            var order = _context.Orders.FirstOrDefault(m => m.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
