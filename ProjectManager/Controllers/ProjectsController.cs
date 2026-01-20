using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    [Authorize] // Only for login users
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Projects getting project list
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.Include(p => p.Customer).ToListAsync();
            return View(projects);
                
        }
        // GET: Projects/Create display empty form
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CompanyName");

            return View();
        }
        // POST: Projects/Create handling data collected in form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Deadline,CustomerId")] Project project)
        {

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //In case validation failled, loading client list again getting back to form
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CompanyName", project.CustomerId);
            return View(project);
        }
               
    }
}
