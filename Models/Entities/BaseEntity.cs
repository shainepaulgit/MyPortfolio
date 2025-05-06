namespace MyPortfolio.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(8);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(8);
    }
    public enum SkillCategories
    {
        Native,
        Framework,
    }
    public enum PostType
    {
        Skill,
        Project,
        ProjectCategory,
        ServiceCategory
    }
    public enum GitHubRequestStatus
    {
        Pending,
        Approved,
        Declined
    }
}
