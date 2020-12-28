﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;
using FreshMart.Database;
using FreshMart.ViewModels;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagerController : Controller
    {
        private readonly AppDbContext _context;

        public OrderManagerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderManager
        [Route("Admin/OrderManager")]
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(o => o.Customer).AsNoTracking();

            var products = _context.Products
                .Include(p => p.District)
                .Include(p => p.Category)
                .Include(p => p.Seller).AsNoTracking()
                .ToList();
            var productOrder = _context.ProductOrders.AsNoTracking().ToList();
            var vm = new AdminOrderViewModel
            {
                Products = products,
                Orders = orders,
                ProductOrders = productOrder,
                AgentOrders = _context.AgentOrders.ToList()
            };

            return View(vm);
        }





        // GET: Admin/OrderManager/Details/5
        [Route("Admin/OrderManager/Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        [Route("Admin/OrderManager/ConfirmOrder/{id}")]
        public IActionResult ConfirmOrder(long id)
        {
            var vm = new AdminOrderViewModel
            {
                Order = _context.Orders.Find(id),

            };
            return View(vm);
        }

        [HttpPost]
        [Route("Admin/OrderManager/Match")]
        public IActionResult Match(AdminOrderViewModel orderVm)
        {
            if (orderVm.MatchVm.AccountNo == null || orderVm.MatchVm.TransactionId == null)
            {
                return RedirectToAction("ConfirmOrder", new { id = orderVm.Order.Id });
            }

            if (orderVm.MatchVm.AccountNo == null && orderVm.MatchVm.TransactionId == null)
            {
                return RedirectToAction("ConfirmOrder", new { id = orderVm.Order.Id });
            }

            var order = _context.Orders.Find(orderVm.Order.Id);

            if (order.AccountNo == orderVm.MatchVm.AccountNo && order.TransactionId == orderVm.MatchVm.TransactionId)
            {
                ViewBag.confirm = "Matched !";
            }

            var vm = new OrderViewModel
            {
                Order = _context.Orders.Find(orderVm.Order.Id)
            };
            return View("ConfirmOrder", vm);

        }



        [Route("Admin/OrderManager/ConfirmOrder/{id}/{approve}")]
        public IActionResult ConfirmOrder(long? id, bool approve)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.SingleOrDefault(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            if (approve == false)
            {
                return NotFound();
            }

            var agentOrder = _context.AgentOrders.Where(c => c.OrderId == order.Id).SingleOrDefault();
            if (approve == true)
            {
                agentOrder.IsPaid = true;
            }

            //order will be modified here


            _context.AgentOrders.Update(agentOrder);
            _context.SaveChanges();

            TempData["payapprove"] = "Payment Successfully approved!";
            return RedirectToAction("Index", "OrderManager");
        }


        // GET: Admin/OrderManager/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            return View(order);
        }

        // POST: Admin/OrderManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CustomerId,SellerId,AccountNo,TransactionId,Name,Email,ShippingAddress,DistrictId,PostalCode,Zip,StreetNo,TotalPrice,IsOrderCompleted,OrderDate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            return View(order);
        }





        // POST: Admin/OrderManager/Delete/5
        [Route("Admin/OrderManager/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
