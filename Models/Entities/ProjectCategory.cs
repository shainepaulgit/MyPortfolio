namespace MyPortfolio.Models.Entities
{
    public class ProjectCategory:BaseEntity
    {
        public string IconName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
