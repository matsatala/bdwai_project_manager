using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/tasks
        // tasks in json form
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.Category)
                .Select(t => new
                {
                    t.Id,
                    t.Topic,
                    t.Progress,
                    Priority = t.Priority,
                    ProjectName = t.Project != null ? t.Project.Title : "Brak projektu",
                    CategoryName = t.Category != null ? t.Category.Name : "Brak kategorii"
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // 2. POST: api/tasks
        // Creating new tasks 
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // verifying is project exists
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists)
            {
                return BadRequest("Podany projekt nie istnieje.");
            }

            // New objected created with data from json
            var newTask = new ProjectTask
            {
                Topic = dto.Topic,
                Progress = dto.Progress,
                Priority = dto.Priority,
                ProjectId = dto.ProjectId,
                CategoryId = dto.CategoryId
            };

            _context.Add(newTask);
            await _context.SaveChangesAsync();

            // Returning 201 Code (Created) 
            return CreatedAtAction(nameof(GetAllTasks), new { id = newTask.Id }, newTask);
        }
    }
    
    public class CreateTaskDto
    {
        public string Topic { get; set; }
        public int Progress { get; set; } // 0 - 100
        public int Priority { get; set; }
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
    }
}