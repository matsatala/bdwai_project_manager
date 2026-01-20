using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;

namespace ProjectManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Rejestracja tabel
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Dane startowe
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kategorie
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Frontend" },
                new Category { Id = 2, Name = "Backend" }
            );

            // Klient
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, CompanyName = "Firma Testowa", ContactEmail = "admin@test.pl", NIP = "0000000000" }
            );

            // Projekt
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Title = "Aplikacja WWW", Description = "Projekt zaliczeniowy", Deadline = new DateTime(2026, 06, 01), CustomerId = 1 }
            );
        }
    }
}