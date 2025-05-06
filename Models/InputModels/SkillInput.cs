using MyPortfolio.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class SkillInput:BaseInput
    {
        [Required]
        public string LogoFileName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required,DisplayName("Skill Percentage")]
        public int SkillPercentage { get; set; }
        [Required,DisplayName("Skill Category")]
        public SkillCategories SkillCategory { get; set; }
    }
}
