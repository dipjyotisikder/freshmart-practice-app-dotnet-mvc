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

namespace FreshMart.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
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

            var customerSingle = _context.Customers.Where(c => c.Email == User.Identity.Name).Single();
            ViewBag.districts = _context.Districts.ToList();

            return View(customerSingle);
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
                    if (!CustomerExists(customer.Id))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", customer.DistrictId);
            return View(customer);
        }






        [Route("Customers/orders")]
        public IActionResult Orders()
        {
            var app = _context.Customers.Where(c => c.Email == User.Identity.Name);
            if (app.SingleOrDefault() == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var customerSingle = _context.Customers.Where(c => c.Email == User.Identity.Name).Single();
            var orders = _context.Orders.Where(c => c.CustomerId == customerSingle.Id).ToList();

            List<AgentOrder> agentOrders = new List<AgentOrder>();
            foreach (var item in orders)
            {
                agentOrders.Add(_context.AgentOrders.Where(c => c.OrderId == item.Id).SingleOrDefault());
            }

            ViewBag.orders = orders;
            ViewBag.agentOrders = agentOrders;
            ViewBag.districts = _context.Districts.ToList();

            return View(orders);
        }






        [Route("Admin/Customers/DeleteOrder/{id}")]

        public IActionResult DeleteOrder(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var orderchk = _context.Orders.Include(c => c.Customer).Where(c => c.Id == id).SingleOrDefault();

            if (User.Identity.Name != orderchk.Customer.Email)
            {
                return NotFound();
            }

            var order = _context.Orders.SingleOrDefault(m => m.Id == id);
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
