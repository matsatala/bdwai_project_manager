using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nazwa firmy jest wymagana")]
        [Display(Name = "Nazwa Firmy")]
        public string CompanyName { get; set; }
        [EmailAddress]
        [Display(Name = "Adres Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "NIP")]
        public string NIP { get; set; }
        [Phone]
        [Display(Name = "Numer Telefonu")]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Project>? Projects { get; set; }
    }
}