using System.ComponentModel.DataAnnotations.Schema;

namespace MyPortfolio.Models.Entities
{
    public class Project:BaseEntity
    {
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectPictureFileName { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsGitHubRepository { get; set; }
        [ForeignKey("ProjectCategory")]
        public int ProjectCategoryId { get; set; }
        public virtual ProjectCategory ProjectCategory { get; set; }

    }
}
