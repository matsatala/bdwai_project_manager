using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;

namespace ProjectManager.Data
{
    public static class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // 1. Sprawdź, czy baza jest utworzona
                context.Database.EnsureCreated();

                // 2. Jeśli są już projekty, nie rób nic (nie dubluj danych)
                if (context.Projects.Any())
                {
                    return; 
                }

                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // --- KROK A: TWORZENIE 3 UŻYTKOWNIKÓW ---
                var users = new List<IdentityUser>();
                for (int i = 1; i <= 3; i++)
                {
                    var email = $"user{i}@test.com";
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                        // Hasło musi spełniać wymogi (Duża litera, cyfra, znak spec.)
                        await userManager.CreateAsync(user, "Haslo123!"); 
                        users.Add(user);
                    }
                    else
                    {
                        users.Add(user);
                    }
                }

                // --- KROK B: TWORZENIE 3 KLIENTÓW ---
                var customers = new List<Customer>
                {
                    new Customer { CompanyName = "TechCorp S.A.", NIP = "1234567890", ContactEmail = "kontakt@techcorp.pl", PhoneNumber = "111-222-333" },
                    new Customer { CompanyName = "SoftHouse Sp. z o.o.", NIP = "0987654321", ContactEmail = "biuro@softhouse.com", PhoneNumber = "444-555-666" },
                    new Customer { CompanyName = "Januszex Import-Export", NIP = "5555555555", ContactEmail = "prezes@januszex.pl", PhoneNumber = "777-888-999" }
                };
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync(); // Zapisz, żeby dostać ich ID

                // --- KROK C: TWORZENIE 4 PROJEKTÓW ---
                var projects = new List<Project>
                {
                    new Project { Title = "System CRM", Description = "Budowa systemu CRM dla klienta", Deadline = DateTime.Now.AddMonths(2), CustomerId = customers[0].Id },
                    new Project { Title = "Sklep Internetowy", Description = "Wdrożenie PrestaShop", Deadline = DateTime.Now.AddMonths(1), CustomerId = customers[1].Id },
                    new Project { Title = "Aplikacja Mobilna", Description = "Appka na iOS i Android", Deadline = DateTime.Now.AddMonths(5), CustomerId = customers[1].Id },
                    new Project { Title = "Strona Wizytówka", Description = "Prosta strona Wordpress", Deadline = DateTime.Now.AddDays(14), CustomerId = customers[2].Id }
                };
                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                // --- KROK D: TWORZENIE ZADAŃ (3 na każdy projekt) ---
                var random = new Random();
                var categories = context.Categories.ToList(); // Pobierz kategorie (Bug, Feature...)

                foreach (var project in projects)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        // Losowy user
                        var randomUser = users[random.Next(users.Count)];
                        // Losowa kategoria (jeśli są)
                        var randomCatId = categories.Any() ? categories[random.Next(categories.Count)].Id : 0;
                        // Jeśli nie ma kategorii w bazie, musisz zadbać, żeby ten kod nie wybuchł, 
                        // ale zakładamy, że kategorie dodały się z poprzedniego kodu w Program.cs

                        context.ProjectTasks.Add(new ProjectTask
                        {
                            Topic = $"Zadanie {j} dla {project.Title}",
                            Description = "Automatycznie wygenerowane zadanie testowe.",
                            Priority = random.Next(1, 4), // 1-3
                            Progress = random.Next(0, 100),
                            ProjectId = project.Id,
                            CategoryId = randomCatId,
                            AssignedUserId = randomUser.Id
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}