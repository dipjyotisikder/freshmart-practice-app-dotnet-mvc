using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshMart.Database;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgentsController : Controller
    {
        private readonly AppDbContext _context;

        public AgentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Agents
        [Route("Admin/Agents")]
        public async Task<IActionResult> Index()
        {
            var agent = await _context.Agents.Include(x => x.User).ThenInclude(x => x.District).AsNoTracking().ToListAsync();
            return View(agent);
        }

        // GET: Admin/Agents/Details/5


        [Route("Admin/Agents/AgentApprove/{id}")]
        public IActionResult AgentApprove(long id)
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
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .Include(x => x.User).ThenInclude(x => x.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }
            return View(agent);
        }


        [Route("Admin/Agents/DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(long id)
        {
            var agent = _context.Agents.SingleOrDefault(m => m.Id == id);
            _context.Agents.Remove(agent);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        private bool AgentExists(long id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}
