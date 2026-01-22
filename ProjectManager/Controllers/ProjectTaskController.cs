using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    [Authorize]
    public class ProjectTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectTasks
        public async Task<IActionResult> Index()
        {
            var tasks = _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.Category)
                .Include(t => t.AssignedUser);
            return View(await tasks.ToListAsync());
        }

        // GET: ProjectTasks/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: ProjectTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask projectTask)
        {
            
            ModelState.Remove("Project");
            ModelState.Remove("Category");
            ModelState.Remove("AssignedUser");

            if (ModelState.IsValid)
            {
                _context.Add(projectTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", projectTask.ProjectId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectTask.CategoryId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Email", projectTask.AssignedUserId);
            return View(projectTask);
        }
    }
}