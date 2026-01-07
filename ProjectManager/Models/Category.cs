namespace ProjectManager.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
    }
}
