using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;


namespace ProjectManager.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects
                .Select(p => new { p.Id,
                    p.Title,
                    p.Description,
                    Deadline = p.Deadline.ToString("yyyy-MM-dd"),
                    CustomerName = p.Customer.CompanyName
                    
                })
                .ToListAsync();
            return Ok(projects);
        }
    }
}
