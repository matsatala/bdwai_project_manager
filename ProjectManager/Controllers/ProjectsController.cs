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
        
        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")] // Tylko Admin wejdzie tutaj
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CompanyName", project.CustomerId);
            return View(project);
        }
        
        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.Id) return NotFound();

            // Fix walidacji (jak wcześniej)
            ModelState.Remove("Customer");
            ModelState.Remove("ProjectTasks");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Projects.Any(e => e.Id == project.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CompanyName", project.CustomerId);
            return View(project);
        }
        
        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null) return NotFound();

            return View(project);
        }

// POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
               
    }
}
