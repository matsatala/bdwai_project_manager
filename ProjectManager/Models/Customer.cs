using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwê firmy")]
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string NIP { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}