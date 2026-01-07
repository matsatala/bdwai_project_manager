using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytu³ jest wymagany")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
    }
}