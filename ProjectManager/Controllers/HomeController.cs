using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // wykorzystanie ToListAsync()
using ProjectManager.Data;          // przestrzeñ danych
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // kontekst bazy danych
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Pobieramy podstawowe statystyki
            var projects = await _context.Projects.ToListAsync();
            var tasks = await _context.ProjectTasks.ToListAsync();

            var model = new DashboardViewModel
            {
                TotalProjects = projects.Count,
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => t.Progress == 100),
                AverageProgress = tasks.Any() ? Math.Round(tasks.Average(t => (double)t.Progress), 1) : 0,

                // 3 najbli¿sze terminy projektów, które jeszcze trwaj¹
                UpcomingDeadlines = projects
                    .Where(p => p.Deadline >= DateTime.Now)
                    .OrderBy(p => p.Deadline)
                    .Take(3)
                    .ToList()
            };

            return View(model);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}