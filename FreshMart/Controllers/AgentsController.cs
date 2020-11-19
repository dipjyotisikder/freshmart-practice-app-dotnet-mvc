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
    public class AgentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgentsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Agents
        [Route("Agents")]
        public IActionResult Index()
        {
            var app = _context.Agents.Where(c => c.Email == User.Identity.Name);
            if (app.ToList().Count < 1)
            {
                return RedirectToAction("AgentRequest", "Agents");
            }

            if (app.FirstOrDefault() != null && app.FirstOrDefault().Approval == false)
            {
                TempData["notapproved"] = "You are not approved yet! Contact with admin.";
                return RedirectToAction("AgentRequest", "Agents");
            }

            var agentSingle = _context.Agents.Where(c => c.Email == User.Identity.Name).Single();
            ViewBag.districts = _context.Districts.ToList();

            return View(agentSingle);
        }


        [HttpGet]
        [Route("Agents/AgentRequest")]
        public IActionResult AgentRequest()
        {
            var already = _context.Agents.Where(c => c.Email == User.Identity.Name);
            if (already.ToList().Count > 0 && already.FirstOrDefault().Approval != false)
            {
                return RedirectToAction("Index", "Agents");
            }

            var vm = new Agent
            {
                Email = User.Identity.Name
            };

            ViewBag.email = User.Identity.Name;
            ViewBag.districts = _context.Districts.ToList();
            return View(vm);
        }


        [HttpPost]
        [Route("Agents/AgentRequest")]
        public IActionResult AgentRequest(Agent agent)
        {
            if (!ModelState.IsValid)
            {
                return View(agent);
            }

            _context.Agents.Add(agent);
            _context.SaveChanges();

            return RedirectToAction("Index", "Agents");
        }


        // POST: Customers/Edit/5
        [HttpPost]
        [Route("Agents/Edit")]
        public IActionResult Edit(Agent agent)
        {
            if (agent.Id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agent);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgentExists(agent.Id))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", agent.DistrictId);
            return View(agent);
        }



        [Route("Agents/orders")]
        public IActionResult Orders()
        {
            var already = _context.Agents.Where(c => c.Email == User.Identity.Name);
            if (already.ToList().Count < 1)
            {
                return RedirectToAction("AgentRequest", "Agents");
            }

            var agentSingle = _context.Agents.Where(c => c.Email == User.Identity.Name).Single();
            var agentOrders = _context.AgentOrders.Where(c => c.AgentId == agentSingle.Id).ToList();

            List<Order> orders = new List<Order>();
            foreach (var item in agentOrders)
            {
                orders.Add(_context.Orders.Find(item.OrderId));
            }

            ViewBag.orders = orders;
            ViewBag.agentOrders = agentOrders;
            ViewBag.districts = _context.Districts.ToList();
            return View(orders);

        }


        [Route("Agents/OrderState/{id}/{sig}")]
        public IActionResult OrderState(int id, string sig)
        {
            var order = _context.Orders.Find(id);
            var agentOrder = _context.AgentOrders.Where(c => c.OrderId == order.Id);

            if (agentOrder.SingleOrDefault() == null)
            {
                TempData["agentordererr"] = "Order Not Found!";
                return RedirectToAction("Orders", "Agents");
            }
            var aOrder = _context.AgentOrders.Where(c => c.OrderId == order.Id).Single();
            switch (sig)
            {
                case "route":

                    aOrder.IsOnRoute = true;
                    _context.AgentOrders.Update(aOrder);
                    _context.SaveChanges();
                    break;

                case "full":

                    aOrder.IsFullyCompleted = true;
                    _context.AgentOrders.Update(aOrder);
                    _context.SaveChanges();


                    order.IsOrderCompleted = true;
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    break;
                default:
                    TempData["AgentOrderParamErr"] = "Parameter Not Found!";
                    return RedirectToAction("Orders", "Agents");
            }


            TempData["AgentOrderSuccess"] = "Order State Changed successfully";
            return RedirectToAction("Orders", "Agents");
        }


        private bool AgentExists(int id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}
