using MyPortfolio.Models.Entities;

namespace MyPortfolio.Models.DTOs
{
    public class DataTable
    {
        public List<Skill> Skills { get; set; }
        public List<Project> Projects { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }
        public List<ServiceCategory> ServiceCategories { get; set; }
    }
}
