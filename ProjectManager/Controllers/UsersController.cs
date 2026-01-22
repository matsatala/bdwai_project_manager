using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data; // Jeśli potrzebne do innych rzeczy, ale tu użyjemy UserManager

namespace ProjectManager.Controllers
{
    
    [Authorize(Roles = "Admin")] // Tylko Admin!
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Lista użytkowników
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Usuwanie - akcja bezpośrednia (bez widoku potwierdzenia dla szybkości, 
        // lub możesz zrobić widok potwierdzenia analogicznie jak w Customers)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // To usunie użytkownika. Dzięki 'OnDelete(SetNull)' w bazie,
                // zadania tego użytkownika staną się "Nieprzypisane", ale nie znikną.
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}