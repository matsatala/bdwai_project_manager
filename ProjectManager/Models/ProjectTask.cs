using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProjectManager.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Temat jest wymagany")]
        public string Topic { get; set; }

        public string Description { get; set; } // OPIS

        [Range(0, 100, ErrorMessage = "Postęp musi być od 0 do 100")]
        public int Progress { get; set; }
        
        public int Priority { get; set; }

        // Relacja z Projektem
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        // Relacja z Kategorią
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public string? AssignedUserId { get; set; } 
        public virtual IdentityUser? AssignedUser { get; set; }
    }
}