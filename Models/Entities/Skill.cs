namespace MyPortfolio.Models.Entities
{
    public class Skill:BaseEntity
    {
        public string LogoFileName { get; set; }
        public string Title { get; set; }
        public int SkillPercentage { get; set; }
        public SkillCategories SkillCategory { get; set; }

    }
}
