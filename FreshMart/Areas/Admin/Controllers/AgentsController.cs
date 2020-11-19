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
    public class AgentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Agents
        [Route("Admin/Agents")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Agents.Include(a => a.District);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Agents/Details/5


        [Route("Admin/Agents/AgentApprove/{id}")]
        public IActionResult AgentApprove(int id)
        {
            var agent = _context.Agents.Find(id);
            agent.Approval = true;

            _context.Agents.Update(agent);
            _context.SaveChanges();

            TempData["approval"] = "You approved " + agent.Name + " successfully !";
            return RedirectToAction("Index", "Agents");
        }





        // GET: Admin/Agents/Delete/5
        [Route("Admin/Agents/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .Include(a => a.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }


        [Route("Admin/Agents/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var agent = _context.Agents.SingleOrDefault(m => m.Id == id);
            _context.Agents.Remove(agent);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool AgentExists(int id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}
