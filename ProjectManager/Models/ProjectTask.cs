using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        public string Topic { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; }
        public int Priority { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}