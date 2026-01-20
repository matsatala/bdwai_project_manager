using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;


namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectsApi
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects
                .Select(p => new { p.Id, p.Title, p.Deadline })
                .ToListAsync();
            return Ok(projects);
        }
    }
}
