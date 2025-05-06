using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPortfolio.Models.InputModels
{
    public class ProjectInput:BaseInput
    {
        [Required,DisplayName("Project Title")]
        public string ProjectTitle { get; set; }
        [Required, DisplayName("Project Description")]
        public string ProjectDescription { get; set; }

        public string ProjectPictureFileName { get; set; }
        [Required, DisplayName("Redirect Url")]
        public string RedirectUrl { get; set; }
        [Required, DisplayName("Is GitHub Repository")]
        public bool IsGitHubRepository { get; set; }
        [Required, DisplayName("Project Category")]
        public int ProjectCategoryId { get; set; }
    }
}
