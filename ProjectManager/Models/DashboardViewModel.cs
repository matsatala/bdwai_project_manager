namespace ProjectManager.Models
{
    public class DashboardViewModel
    {
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double AverageProgress { get; set; }
        // Lista projektów z najbliższym terminem, reszta jak nazwa wskazuje
        public List<Project> UpcomingDeadlines { get; set; } = new List<Project>();
    }
}