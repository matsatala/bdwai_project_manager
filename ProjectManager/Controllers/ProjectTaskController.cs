using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using System.Security.Claims;

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
        public async Task<IActionResult> Index(int? projectId, bool onlyMine = false)
        {
            var tasksQuery = _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.Category)
                .Include(t => t.AssignedUser)
                .AsQueryable();
           
            //If id has value we filter tasks by it
            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);

                // get name of project 
                var project = await _context.Projects.FindAsync(projectId);
                ViewData["ProjectTitle"] = project?.Title;
                ViewData["ProjectId"] = projectId;
            }
            if (onlyMine)
            {
                // id of current user
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId != null)
                {
                    tasksQuery = tasksQuery.Where(t => t.AssignedUserId == currentUserId);
                    ViewData["FilterInfo"] = "Wyœwietlam tylko Twoje zadania";
                }
            }
            return View(await tasksQuery.ToListAsync());
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

        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", projectTask.ProjectId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectTask.CategoryId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Email", projectTask.AssignedUserId);

            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask projectTask)
        {
            if (id != projectTask.Id)
            {
                return NotFound();
            }

            
            ModelState.Remove("Project");
            ModelState.Remove("Category");
            ModelState.Remove("AssignedUser");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProjectTasks.Any(e => e.Id == projectTask.Id))
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

            // Gdy failed prze³adujemy i do widoku elo
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", projectTask.ProjectId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectTask.CategoryId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Email", projectTask.AssignedUserId);
            return View(projectTask);
        }
    }
}